﻿using System.Windows.Media;
using MahApps.Metro.Controls;
using NpcChat.ViewModels;
using NpcChatSystem;
using NpcChatSystem.Data;
using NpcChatSystem.Data.DialogTreeItems;

namespace NpcChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            DataContext = new WindowViewModel();
            InitializeComponent();
        }
    }
}