using Improvisation.Library;
using Improvisation.Library.Music;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Improvisation.FinalUI
{
    public partial class FInalUICreateStatModel : Form
    {
        private string[] files;
        private bool homogeneous { get { return this.leftRangeNumericUpDown.Value == this.rightRangeNumericUpDown.Value; } }

        public FInalUICreateStatModel()
        {
            InitializeComponent();

            this.beginGenerationButton.Enabled = false;
        }

        private void loadMidiFilesButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog()
            {
                Filter = "Midi Files (*.mid)|*.mid",
                Multiselect = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.files = openFileDialog1.FileNames;

                this.midiFileListView.Items.AddRange(this.files.Select(x => new ListViewItem(FinalUIHelperMethods.FileFriendlyString(x))).ToArray());
                this.beginGenerationButton.Enabled = true;
            }
        }

        private void beginGenerationButton_Click(object sender, EventArgs e)
        {
            try
            {
                var retriever = new PianoNoteRetriever();
                var midiEvents = new InstrumentMidiEventProducer(this.files.Select(x => new Sequence(x)));
                IReadOnlyList<MidiEvent> midi = midiEvents.GetOrderedMessages(GeneralMidiInstrument.AcousticGrandPiano);

                Chord.AllowForComplexSimplification = this.checkBox1.Checked;
                var accords = Chord.RetrieveChords(midi, retriever);

                INGrams<Chord> grams = null;
                if (this.homogeneous)
                {
                    grams = HomogenousNGrams<Chord>.BuildNGrams((int)this.leftRangeNumericUpDown.Value, accords);
                }
                else
                {
                    grams = HeterogenousNGrams<Chord>.BuildNGrams((int)this.leftRangeNumericUpDown.Value, (int)this.rightRangeNumericUpDown.Value, accords);
                }

                NGramGraphMarkovChain<Chord> graph = new NGramGraphMarkovChain<Chord>(grams);
                this.Save(graph);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void leftRangeNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (this.rightRangeNumericUpDown.Value < this.leftRangeNumericUpDown.Value)
            {
                MessageBox.Show("left must be less or equal to right");
                this.leftRangeNumericUpDown.Value = this.rightRangeNumericUpDown.Value;
            }
        }

        private void rightRangeNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (this.rightRangeNumericUpDown.Value < this.leftRangeNumericUpDown.Value)
            {
                MessageBox.Show("left must be less or equal to right");
                this.leftRangeNumericUpDown.Value = this.rightRangeNumericUpDown.Value;
            }
        }

        private void Save(NGramGraphMarkovChain<Chord> a)
        {
            TemporaryVariables.Graph = a;
            this.Dispose();
        }
    }
}