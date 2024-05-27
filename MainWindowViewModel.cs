using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfApp2
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel()
        {
            DispatcherTimer timer = new()
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            timer.Tick += this.GetClipboardValue;
            timer.Start();
        }

        private int m_count;

        private void GetClipboardValue(object? sender, EventArgs e)
        {
            Debug.WriteLine($"{this.m_count}");
            this.m_count++;
            this.Items = this.GetClipboardData();
            Debug.WriteLine($"exit: {this.m_count}");
        }

        private IEnumerable<Item> GetClipboardData()
        {

            var dataFormats = typeof(DataFormats).GetFields(BindingFlags.Public | BindingFlags.Static)
                                           .Select(f => f.Name)
                                           .ToList();
            var dataFormatsInClipboard =
                         dataFormats.Where(df => Clipboard.ContainsData(df))
                         .ToList();
            return dataFormatsInClipboard.Select(
                f => new Item(f, Clipboard.GetData(f)?.ToString() ?? "null"));
        }

        public string Hoge => "manually published property";

        [ObservableProperty]
        private string m_bitmapText;

        [ObservableProperty]
        private IEnumerable<Item> m_items = Enumerable.Empty<Item>(); 

        [ObservableProperty]
        private string m_selectedItem;
    }

    public struct Item
    {
        public Item(string label, string value)
        {
            this.Label = label;
            this.Value = value;
        }
        public readonly string Label { get;}
        public readonly string Value { get; }
    }
}
