﻿using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using NpcChatSystem.Annotations;
using NpcChatSystem.System.TypeStore;

namespace NpcChatSystem.Data.Dialog.DialogParts
{
    /// <summary>
    /// Piece of text
    /// </summary>
    [DebuggerDisplay("{Text}"), Export(typeof(IDialogElement)), NiceTypeName(c_elementName)]
    public class DialogText : IDialogElement, INotifyPropertyChanged
    {
        private const string c_elementName = "Plain Text";
        public string ElementName => c_elementName;

        public string Text
        {
            get => m_text;
            set
            {
                m_text = value;
                RaiseChanged();
            }
        }

        private string m_text = "";


        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void RaiseChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
