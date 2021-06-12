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
using System.Windows.Threading;

namespace MatchGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthOfSecondsElapsed;
        int matchesFound;
        bool gameOver = false;
        List<float> scores = new List<float>();

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthOfSecondsElapsed++;
            timeTextBlock.Text = (30 - tenthOfSecondsElapsed / 10f).ToString("0.0s");
            if (matchesFound == 8 || tenthOfSecondsElapsed >= 300)
            {
                timer.Stop();
                gameOver = true;
                if (tenthOfSecondsElapsed >= 300)
                {
                    timeTextBlock.Text = "Time ran out! Click to play again";
                }
                else
                {
                    timeTextBlock.Text = "Score: " + scores.Sum() + " - Click to play again";
                }
                
            }
        }

        private void SetUpGame()
        {
            tenthOfSecondsElapsed = 0;
            gameOver = false;

            List<string> allAnimalEmojis = new List<string>()
            {
                "🐒", "🦍", "🐩", "🦏", "🐄", "🐖", "🐪", "🦘", "🐎", "🦙", "🦨", "🐘", "🐁", "🐇", "🐢", "🐍", "🦀", "🐓", "🦆", "🦅"
            };

            List<string> animalEmoji = new List<string>();

            Random random = new Random();

            // Adds two of a random emoji from the all animals list to the animal list to ensure even pairs when emojis are assigned to text blocks.
            foreach (int value in Enumerable.Range(0, 8))
            {
                int index = random.Next(allAnimalEmojis.Count);
                string nextEmoji = allAnimalEmojis[index];
                animalEmoji.Add(nextEmoji);
                animalEmoji.Add(nextEmoji);
                allAnimalEmojis.RemoveAt(index);
            }

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                    textBlock.Visibility = Visibility.Visible;
                }

                timer.Start();
                tenthOfSecondsElapsed = 0;
                matchesFound = 0;
            }
        }

        TextBlock lasTextBlockClicked;
        bool findingMatch = false;
        

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if (!gameOver)
            {
                if (!findingMatch)
                {
                    textBlock.Visibility = Visibility.Hidden;
                    lasTextBlockClicked = textBlock;
                    findingMatch = true;
                }
                else if (textBlock.Text == lasTextBlockClicked.Text)
                {
                    matchesFound++;
                    scores.Add(30 - tenthOfSecondsElapsed / 10f);
                    textBlock.Visibility = Visibility.Hidden;
                    findingMatch = false;
                }
                else
                {
                    lasTextBlockClicked.Visibility = Visibility.Visible;
                    findingMatch = false;
                }
            }


        }

        private void timeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8 || tenthOfSecondsElapsed >= 300)
            {
                SetUpGame();
            }
        }
    }
}
