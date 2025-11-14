using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Threading;

namespace JetTimer
{
    public partial class MainWindow : Window
    {
        // Global hotkey imports
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int HOTKEY_ID_F1 = 9000;
        private const int HOTKEY_ID_F2 = 9001;
        private const uint VK_F1 = 0x70;
        private const uint VK_F2 = 0x71;

        private IntPtr _windowHandle;
        private HwndSource? _source;

        // Timer 1 state
        private DispatcherTimer? _timer1;
        private int _timeLeft1 = 87;
        private bool _isTimer1Active = false;

        // Timer 2 state
        private DispatcherTimer? _timer2;
        private int _timeLeft2 = 87;
        private bool _isTimer2Active = false;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Register global hotkeys
            _windowHandle = new WindowInteropHelper(this).Handle;
            _source = HwndSource.FromHwnd(_windowHandle);
            _source.AddHook(HwndHook);

            RegisterHotKey(_windowHandle, HOTKEY_ID_F1, 0, VK_F1);
            RegisterHotKey(_windowHandle, HOTKEY_ID_F2, 0, VK_F2);

            // Position window in top-right corner
            var workArea = SystemParameters.WorkArea;
            Left = workArea.Right - Width - 20;
            Top = 20;
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            // Unregister hotkeys
            UnregisterHotKey(_windowHandle, HOTKEY_ID_F1);
            UnregisterHotKey(_windowHandle, HOTKEY_ID_F2);
            _source?.RemoveHook(HwndHook);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            if (msg == WM_HOTKEY)
            {
                int id = wParam.ToInt32();
                if (id == HOTKEY_ID_F1)
                {
                    StartTimer(1);
                    handled = true;
                }
                else if (id == HOTKEY_ID_F2)
                {
                    StartTimer(2);
                    handled = true;
                }
            }
            return IntPtr.Zero;
        }

        private void StartTimer(int timerNum)
        {
            if (timerNum == 1)
            {
                // Stop existing timer 1 if running
                _timer1?.Stop();

                // Reset timer 1
                _timeLeft1 = 87;
                _isTimer1Active = true;
                UpdateTimerDisplay(1);
                UpdateTimerColor(1, _timeLeft1);
                UpdateMainContainer();

                // Start countdown
                _timer1 = new DispatcherTimer();
                _timer1.Interval = TimeSpan.FromSeconds(1);
                _timer1.Tick += (s, e) =>
                {
                    _timeLeft1--;
                    UpdateTimerDisplay(1);
                    UpdateTimerColor(1, _timeLeft1);

                    if (_timeLeft1 <= 0)
                    {
                        _timer1.Stop();
                        _isTimer1Active = false;

                        // Wait 2 seconds then reset display
                        var resetTimer = new DispatcherTimer();
                        resetTimer.Interval = TimeSpan.FromSeconds(2);
                        resetTimer.Tick += (s2, e2) =>
                        {
                            Timer1Display.Text = "--";
                            UpdateTimerColor(1, 0);
                            UpdateMainContainer();
                            resetTimer.Stop();
                        };
                        resetTimer.Start();
                    }
                };
                _timer1.Start();
            }
            else if (timerNum == 2)
            {
                // Stop existing timer 2 if running
                _timer2?.Stop();

                // Reset timer 2
                _timeLeft2 = 87;
                _isTimer2Active = true;
                UpdateTimerDisplay(2);
                UpdateTimerColor(2, _timeLeft2);
                UpdateMainContainer();

                // Start countdown
                _timer2 = new DispatcherTimer();
                _timer2.Interval = TimeSpan.FromSeconds(1);
                _timer2.Tick += (s, e) =>
                {
                    _timeLeft2--;
                    UpdateTimerDisplay(2);
                    UpdateTimerColor(2, _timeLeft2);

                    if (_timeLeft2 <= 0)
                    {
                        _timer2.Stop();
                        _isTimer2Active = false;

                        // Wait 2 seconds then reset display
                        var resetTimer = new DispatcherTimer();
                        resetTimer.Interval = TimeSpan.FromSeconds(2);
                        resetTimer.Tick += (s2, e2) =>
                        {
                            Timer2Display.Text = "--";
                            UpdateTimerColor(2, 0);
                            UpdateMainContainer();
                            resetTimer.Stop();
                        };
                        resetTimer.Start();
                    }
                };
                _timer2.Start();
            }
        }

        private void UpdateTimerDisplay(int timerNum)
        {
            if (timerNum == 1)
            {
                Timer1Display.Text = _timeLeft1.ToString();
            }
            else if (timerNum == 2)
            {
                Timer2Display.Text = _timeLeft2.ToString();
            }
        }

        private void UpdateTimerColor(int timerNum, int timeLeft)
        {
            TextBlock display = timerNum == 1 ? Timer1Display : Timer2Display;

            // Create or get the glow effect
            DropShadowEffect? glow = display.Effect as DropShadowEffect;
            if (glow == null)
            {
                glow = new DropShadowEffect
                {
                    BlurRadius = 10,
                    ShadowDepth = 0
                };
                display.Effect = glow;
            }

            if (timeLeft <= 0)
            {
                // Inactive
                display.Foreground = new SolidColorBrush(Color.FromRgb(0x55, 0x55, 0x55));
                glow.Color = Colors.Transparent;
            }
            else if (timeLeft <= 10)
            {
                // Danger - Red
                display.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0x00, 0x00));
                glow.Color = Color.FromRgb(0xFF, 0x00, 0x00);
            }
            else if (timeLeft <= 30)
            {
                // Warning - Orange
                display.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xAA, 0x00));
                glow.Color = Color.FromRgb(0xFF, 0xAA, 0x00);
            }
            else
            {
                // Active - Green
                display.Foreground = new SolidColorBrush(Color.FromRgb(0x00, 0xFF, 0x00));
                glow.Color = Color.FromRgb(0x00, 0xFF, 0x00);
            }
        }

        private void UpdateMainContainer()
        {
            // Show container if either timer is active
            if (_isTimer1Active || _isTimer2Active)
            {
                MainContainer.Visibility = Visibility.Visible;
            }
            else
            {
                MainContainer.Visibility = Visibility.Collapsed;
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Allow dragging the window
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
