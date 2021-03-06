﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using NpcChatSystem.Data.Dialog.DialogParts;
using NpcChatSystem.Data.Util;
using NpcChatSystem.Identifiers;

namespace NpcChatSystem.Data.Dialog
{
    /// <summary>
    /// Piece of dialog from within a larger conversation
    /// </summary>
    [DebuggerDisplay("{CharacterId}: {Text}")]
    public class DialogSegment : ProjectNotificationObject
    {
        /// <summary>
        /// Id of the dialog
        /// </summary>
        public DialogSegmentIdentifier Id { get; }

        /// <summary>
        /// Who is expressing this dialog?
        /// </summary>
        public int CharacterId
        {
            get => m_characterId;
            set
            {
                m_characterId = value;
                RaisePropertyChanged();
            }
        }

        public string Text
        {
            get
            {
                StringBuilder text = new StringBuilder();

                foreach (IDialogElement element in m_dialogParts)
                {
                    text.Append(element.Text);
                }

                return text.ToString();
            }
        }

        public IReadOnlyList<IDialogElement> SegmentParts => m_dialogParts;

        private readonly ObservableCollection<IDialogElement> m_dialogParts = new ObservableCollection<IDialogElement>();
        private int m_characterId;

        internal DialogSegment(NpcChatProject project, DialogTreeBranchIdentifier treeId, int dialogId, int charId)
            : base(project)
        {
            Id = new DialogSegmentIdentifier(project[treeId], dialogId);
            CharacterId = charId;
            m_dialogParts.CollectionChanged += PartsChanged;

            m_dialogParts.Add(new DialogText { Text = "Before Before Before Before Before Before " });
            m_dialogParts.Add(new DialogCharacterTrait(m_project, charId));
            m_dialogParts.Add(new DialogText { Text = " after after after after after after" });
        }

        public void AddDialogElement(IDialogElement element)
        {
            m_dialogParts.Add(element);
        }

        public void RemoveDialogElement(IDialogElement element)
        {
            m_dialogParts.Remove(element);
        }

        public void ClearElements()
        {
            m_dialogParts.Clear();
        }

        private void PartsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    IDialogElement element = item as IDialogElement;
                    if (element != null) element.PropertyChanged += PartChanged;
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    IDialogElement element = item as IDialogElement;
                    if (element != null) element.PropertyChanged -= PartChanged;
                }
            }

            RaisePropertyChanged(nameof(SegmentParts));
        }

        private void PartChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(SegmentParts));
            RaisePropertyChanged(nameof(Text));
        }

        public void ChangeCharacter(int newChar)
        {
            if (CharacterId == newChar) return;

            if (m_project.ProjectCharacters.HasCharacter(newChar))
            {
                CharacterId = newChar;
            }
        }

        public static implicit operator DialogTreeIdentifier(DialogSegment d) => d.Id;
        public static implicit operator DialogTreeBranchIdentifier(DialogSegment d) => d.Id;
        public static implicit operator DialogSegmentIdentifier(DialogSegment d) => d.Id;
    }
}
