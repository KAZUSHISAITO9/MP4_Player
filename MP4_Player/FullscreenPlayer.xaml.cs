using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MP4_Player
{
    public partial class FullscreenPlayer : Window
    {
        private List<string> videoFiles;
        private int currentVideoIndex = 0;
        private bool isPlaying = false;

        public FullscreenPlayer(List<string> videos)
        {
            InitializeComponent();
            videoFiles = videos;
            this.WindowState = WindowState.Maximized;
            this.Loaded += FullscreenPlayer_Loaded;
            mediaElement.MediaEnded += MediaElement_MediaEnded;
        }

        private void FullscreenPlayer_Loaded(object sender, RoutedEventArgs e)
        {
            PlayCurrentVideo();
        }

        private void PlayCurrentVideo()
        {
            if (currentVideoIndex >= 0 && currentVideoIndex < videoFiles.Count)
            {
                string videoPath = videoFiles[currentVideoIndex];
                titleTextBlock.Text = Path.GetFileName(videoPath);
                mediaElement.Source = new Uri(videoPath);
                mediaElement.Play();
                isPlaying = true;
            }
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            PlayNextVideo();
        }

        private void PlayNextVideo()
        {
            currentVideoIndex++;
            if (currentVideoIndex >= videoFiles.Count)
            {
                currentVideoIndex = 0; // ループ
            }
            PlayCurrentVideo();
        }

        private void PlayPreviousVideo()
        {
            currentVideoIndex--;
            if (currentVideoIndex < 0)
            {
                currentVideoIndex = videoFiles.Count - 1;
            }
            PlayCurrentVideo();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    if (isPlaying)
                    {
                        mediaElement.Pause();
                        isPlaying = false;
                    }
                    else
                    {
                        mediaElement.Play();
                        isPlaying = true;
                    }
                    e.Handled = true;
                    break;

                case Key.Up:
                    // 音量アップ
                    mediaElement.Volume = Math.Min(1.0, mediaElement.Volume + 0.1);
                    e.Handled = true;
                    break;

                case Key.Down:
                    // 音量ダウン
                    mediaElement.Volume = Math.Max(0.0, mediaElement.Volume - 0.1);
                    e.Handled = true;
                    break;

                case Key.Right:
                    // 次の動画へ
                    PlayNextVideo();
                    e.Handled = true;
                    break;

                case Key.Left:
                    // 前の動画へ
                    PlayPreviousVideo();
                    e.Handled = true;
                    break;

                case Key.M:
                    // メインウィンドウに戻る
                    ReturnToMainWindow();
                    e.Handled = true;
                    break;

                case Key.Escape:
                    // ESCで終了
                    this.Close();
                    e.Handled = true;
                    break;
            }
        }

        private void ReturnToMainWindow()
        {
            mediaElement.Stop();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
