using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DictionaryMeta;

namespace DictionaryVisualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DictionaryMetadata<string, Book> metadata;

        public MainWindow()
        {
            InitializeComponent();

            var book1 = new Book("J.R.R. Tolkien", "The Lord of the Rings");
            var book2 = new Book("Patrick Rothfuss", "Name of the Wind");
            var book3 = new Book("Frank Herbert", "Dune");

            var dict = new Dictionary<string, Book>(5)
            {
                { book1.Author, book1},
                { book2.Author, book2},
                { book3.Author, book3}

            };

            var extractor = new DictionaryMetadataExtractor<string, Book>();

            metadata = extractor.ExtractMetadata(dict);

            this.DrawVisualization(metadata);
        }

        private void DrawVisualization<TKey, TValue>(DictionaryMetadata<TKey, TValue> metadata)
        {
            this.InitGridRowDefinitions(metadata.Buckets.Length);

            for (int i = 0; i < metadata.Buckets.Length; i++)
            {
                AddIndexTextBlock<TKey, TValue>(i);

                AddBucketElement(metadata, i);

                AddEntryElement(metadata, i);
            }

            this.DrawArrows(metadata);
        }

        private void DrawArrows<TKey, TValue>(DictionaryMetadata<TKey, TValue> metadata)
        {
            ArrowsCanvas.Children.Clear();

            for (int i = 0; i < metadata.Buckets.Length; i++)
            {
                if (metadata.Buckets[i] != -1)
                {
                    var bucket = GetElement(i, 2);
                    Point bucketPos = bucket.TransformToAncestor(this.RootGrid).Transform(new Point(0, 0));

                    var entry = GetElement(metadata.Buckets[i], 4);
                    Point entryPos = entry.TransformToAncestor(this.RootGrid).Transform(new Point(0, 0));

                    var line = new Line
                    {
                        X1 = bucketPos.X + bucket.ActualWidth,
                        Y1 = bucketPos.Y + bucket.ActualHeight / 2,
                        X2 = entryPos.X,
                        Y2 = entryPos.Y + entry.ActualHeight / 2,
                        Stroke = Brushes.Blue,
                        StrokeThickness = 4
                    };

                    ArrowsCanvas.Children.Add(line);

                    var head = new Ellipse
                    {
                        Fill = Brushes.Blue,
                        Width = 14,
                        Height = 14
                    };

                    Canvas.SetTop(head, entryPos.Y + entry.ActualHeight / 2 - 7);
                    Canvas.SetLeft(head, entryPos.X - 7);

                    ArrowsCanvas.Children.Add(head);
                }

                if (metadata.Entries[i].Key != null && metadata.Entries[i].Next != -1)
                {
                    var entry1 = GetElement(i, 4);
                    Point entry1Pos = entry1.TransformToAncestor(this.RootGrid).Transform(new Point(0, 0));

                    var entry2 = GetElement(metadata.Entries[i].Next, 4);
                    Point entry2Pos = entry2.TransformToAncestor(this.RootGrid).Transform(new Point(0, 0));

                    var X1 = entry1Pos.X + entry1.ActualWidth;
                    var Y1 = entry1Pos.Y + entry1.ActualHeight / 2;
                    var X2 = entry2Pos.X + entry2.ActualWidth;
                    var Y2 = entry2Pos.Y + entry2.ActualHeight / 2;

                    var path = new Path
                    {
                        Data = new PathGeometry
                        {
                            Figures = new PathFigureCollection
                            {
                                new PathFigure
                                {
                                    Segments = new PathSegmentCollection
                                    {
                                        new PolyBezierSegment
                                        {
                                            Points = new PointCollection
                                            {
                                                new Point(60, (Y2 - Y1) / 3),
                                                new Point(60, (Y2 - Y1) * 2 / 3),
                                                new Point(0, Y2 - Y1),
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        StrokeThickness = 4,
                        Stroke = Brushes.Green
                    };

                    Canvas.SetLeft(path, X1);
                    Canvas.SetTop(path, Y1);

                    ArrowsCanvas.Children.Add(path);

                    var head = new Ellipse
                    {
                        Fill = Brushes.Green,
                        Width = 14,
                        Height = 14
                    };

                    Canvas.SetLeft(head, X2 - 7);
                    Canvas.SetTop(head, Y2 - 7);

                    ArrowsCanvas.Children.Add(head);
                }
            }
        }

        private FrameworkElement GetElement(int row, int column)
        {
            foreach (var element in this.EntriesGrid.Children)
            {
                if (Grid.GetColumn((FrameworkElement)element) == column && Grid.GetRow((FrameworkElement)element) - 1 == row)
                {
                    return (FrameworkElement)element;
                }
            }

            throw new ArgumentException();
        }

        private void AddEntryElement<TKey, TValue>(DictionaryMetadata<TKey, TValue> metadata, int i)
        {
            var entry = new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Height = 50
            };

            if (metadata.Entries[i].Value != null)
            {
                var entryGrid = new Grid();
                entryGrid.ColumnDefinitions.Add(new ColumnDefinition());
                entryGrid.ColumnDefinitions.Add(new ColumnDefinition());
                entryGrid.RowDefinitions.Add(new RowDefinition());
                entryGrid.RowDefinitions.Add(new RowDefinition());
                entryGrid.RowDefinitions.Add(new RowDefinition());

                var entryHashCode = new TextBlock
                {
                    Text = "HashCode: " + metadata.Entries[i].HashCode.ToString(),
                    FontSize = 12,
                    Margin = new Thickness(12, 0, 12, 0)
                };

                var entryModulus = new TextBlock
                {
                    Text = "HashCode mod: " + metadata.Entries[i].HashCode % metadata.Buckets.Length,
                    FontSize = 12,
                    Margin = new Thickness(12, 0, 12, 0)
                };

                var entryKey = new TextBlock
                {
                    Text = "Key: " + metadata.Entries[i].Key.ToString(),
                    FontSize = 12,
                    Margin = new Thickness(12, 0, 12, 0)
                };

                var entryNext = new TextBlock
                {
                    Text = "Next: " + metadata.Entries[i].Next.ToString(),
                    FontSize = 12,
                    Margin = new Thickness(0, 0, 12, 0)
                };

                var entryValue = new TextBlock
                {
                    Text = "Value: " + metadata.Entries[i].Value.ToString(),
                    FontSize = 12,
                    Margin = new Thickness(0, 0, 12, 0)
                };

                Grid.SetColumn(entryKey, 0);
                Grid.SetRow(entryKey, 0);
                Grid.SetColumn(entryHashCode, 0);
                Grid.SetRow(entryHashCode, 1);
                Grid.SetColumn(entryModulus, 0);
                Grid.SetRow(entryModulus, 2);
                Grid.SetColumn(entryNext, 1);
                Grid.SetRow(entryNext, 0);
                Grid.SetColumn(entryValue, 1);
                Grid.SetRow(entryValue, 1);

                entryGrid.Children.Add(entryKey);
                entryGrid.Children.Add(entryHashCode);
                entryGrid.Children.Add(entryModulus);
                entryGrid.Children.Add(entryNext);
                entryGrid.Children.Add(entryValue);

                entry.Child = entryGrid;
            }

            Grid.SetColumn(entry, 4);
            Grid.SetRow(entry, i + 1);
            this.EntriesGrid.Children.Add(entry);
        }

        private void AddBucketElement<TKey, TValue>(DictionaryMetadata<TKey, TValue> metadata, int i)
        {
            var bucket = new Border
            {
                Child = new TextBlock
                {
                    Text = metadata.Buckets[i].ToString()
                },
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Width = 50,
                Height = 50
            };

            Grid.SetColumn(bucket, 2);
            Grid.SetRow(bucket, i + 1);
            this.EntriesGrid.Children.Add(bucket);
        }

        private void AddIndexTextBlock<TKey, TValue>(int i)
        {
            var textBlock = new TextBlock
            {
                Text = i.ToString()
            };

            Grid.SetColumn(textBlock, 0);
            Grid.SetRow(textBlock, i + 1);
            this.EntriesGrid.Children.Add(textBlock);
        }

        private void InitGridRowDefinitions(int rowCount)
        {
            this.EntriesGrid.RowDefinitions.Clear();

            // Extra row for the header.
            this.EntriesGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            this.AddHeaderText(0, "Index");
            this.AddHeaderText(2, "Buckets");
            this.AddHeaderText(4, "Entries");

            for (int i = 0; i < rowCount; i++)
            {
                this.EntriesGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }
        }

        private void AddHeaderText(int column, string text)
        {
            var textBlock = new TextBlock
            {
                Text = text,
                Margin = new Thickness(0, 0, 0, 8)
            };

            Grid.SetColumn(textBlock, column);

            this.EntriesGrid.Children.Add(textBlock);
        }

        private void EntriesGrid_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawArrows(metadata);
        }
    }
}
