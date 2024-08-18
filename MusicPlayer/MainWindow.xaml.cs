using MaterialDesignThemes.Wpf;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SpotifyApiService _spotifyApiService;
        private DispatcherTimer timer;
        private bool isPlaying = false;
        private double currentTime = 0; // Current playback time in seconds
        private double totalTime = 240; // Total duration of the song in seconds

        public MainWindow()
        {
            InitializeComponent();
            _spotifyApiService = new SpotifyApiService();
            Loaded += Window_Loaded;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize Spotify API Service
            await _spotifyApiService.InitializeAsync("d563624981fe41cab2eb46bae03da2d6", "f744a49fcf03445c894a3441b15f9b36");

            // Example: Fetch a specific track
            var track = await _spotifyApiService.GetTrackAsync("3n3Ppam7vgaVa1iaRUc9Lp");
            //MessageBox.Show($"Track: {track.Name} by {track.}");
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Kích hoạt sự kiện nhấn nút tìm kiếm khi Enter được nhấn
                SearchButton_Click(sender, e);
            }
        }

        private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchButton_Click(sender, e);
            }
        }

        // Event handler example for searching tracks
        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var query = searchTextBox.Text;
            if (!string.IsNullOrEmpty(query))
            {
                var searchResponse = await _spotifyApiService.SearchTracksAsync(query);
                var tracks = searchResponse.Tracks.Items.Select((track, index) => new Song
                {
                    Index = index + 1,
                    Title = track.Name,
                    Artist = string.Join(", ", track.Artists.Select(artist => artist.Name)),
                    Duration = TimeSpan.FromMilliseconds(track.DurationMs).ToString(@"mm\:ss"),
                    ImageUrl = track.Album.Images.FirstOrDefault()?.Url,
                    ExternalUrl = track.ExternalUrls["spotify"] // Lấy URL đến trang Spotify
                }).ToList();

                songListView.ItemsSource = tracks;
            }
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isPlaying)
            {
                currentTime += 1;
                if (currentTime >= totalTime)
                {
                    currentTime = totalTime;
                    isPlaying = false;
                    PlayPauseIcon.Kind = PackIconKind.Play; // Change icon to Play
                }
                UpdatePlayerUI();
            }
        }

        private void UpdatePlayerUI()
        {
            if (totalTime > 0)
            {
                timeSlider.Value = (currentTime / totalTime) * 100;
                timeDisplay.Text = $"{TimeSpan.FromSeconds(currentTime):mm\\:ss} / {TimeSpan.FromSeconds(totalTime):mm\\:ss}";
            }
        }



        private void TimeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isPlaying)
            {
                // Seek to the new time in the song
                currentTime = (timeSlider.Value / 100) * totalTime;
                UpdatePlayerUI();
            }
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (isPlaying)
            {
                timer.Stop();
                PlayPauseIcon.Kind = PackIconKind.Play;
            }
            else
            {
                timer.Start();
                PlayPauseIcon.Kind = PackIconKind.Pause;
            }
            isPlaying = !isPlaying;
        }

        private void SkipPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            // Implement skip previous functionality
        }

        private void SkipNextButton_Click(object sender, RoutedEventArgs e)
        {
            // Implement skip next functionality
        }

        private void SongListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (songListView.SelectedItem is Song selectedSong)
            {
                // Cập nhật hình ảnh và tiêu đề của bài hát
                nowPlayingImage.Source = new BitmapImage(new Uri(selectedSong.ImageUrl));
                nowPlayingTitle.Text = selectedSong.Title;

                // Mở liên kết đến trang Spotify
                if (!string.IsNullOrEmpty(selectedSong.ExternalUrl))
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = selectedSong.ExternalUrl,
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error opening the song: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("External URL is not available for this song.");
                }
            }
        }






    }
}