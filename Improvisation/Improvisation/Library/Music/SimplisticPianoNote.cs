using Sanford.Multimedia;
using Sanford.Multimedia.Midi;
using System;

namespace Improvisation.Library.Music
{
    public class SimplisticPianoNote : IEquatable<SimplisticPianoNote>
    {
        public static readonly PianoNoteRetriever PianoNoteRetriever;

        public readonly Note Note;
        public readonly ChannelMessage Message;
        public ChannelCommand Type { get { return this.Message.Command; } }

        static SimplisticPianoNote()
        {
            SimplisticPianoNote.PianoNoteRetriever = new PianoNoteRetriever();
        }
        public SimplisticPianoNote(ChannelMessage message)
        {
            message.NullCheck();
            SimplisticPianoNote.PianoNoteRetriever.IndexInRange((int)message.MidiChannel).AssertTrue();

            this.Note = SimplisticPianoNote.PianoNoteRetriever.RetrieveNote(message).Note;
            this.Message = message;
        }

        public override int GetHashCode()
        {
            return this.Note.GetHashCode() ^ (this.Type.GetHashCode() * 29);
        }
        public override string ToString()
        {
            return this.Note.ToString();
        }

        public static bool operator ==(SimplisticPianoNote one, SimplisticPianoNote second)
        {
            return one.Equals(second);
        }
        public static bool operator !=(SimplisticPianoNote one, SimplisticPianoNote second)
        {
            return !one.Equals(second);
        }

        public override bool Equals(object _obj)
        {
            var obj = (_obj as SimplisticPianoNote);
            if (obj != null)
            {
                return this.Equals(obj);
            }
            else
            {
                return base.Equals(_obj);
            }
        }

        public bool Equals(SimplisticPianoNote other)
        {
            return this.Note == other.Note && this.Type == other.Type;
        }

    }
}
