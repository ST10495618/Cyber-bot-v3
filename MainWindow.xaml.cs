using System;
using System.IO;
using System.Media;
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

namespace WpfApp1
{


    public partial class MainWindow : Window
    {
        private List<QuizQuestion> quiz = new List<QuizQuestion>();
        private int currentQuestion = 0;
        private int score = 0;
        private bool quizRunning = false;
        private bool waitingForReminder = false;
        private int lastTaskId = 0;
        BotDatabase db = new BotDatabase();
        private string favouriteTopic = "";
        private ChatMemory memory = new ChatMemory();
        private List<string> activityLog = new List<string>();

        private Random random = new Random();

        private List<Brush> backgrounds =
            new List<Brush>()
        {
            Brushes.DarkSlateBlue,
            Brushes.DarkOliveGreen,
            Brushes.DarkCyan,
            Brushes.DarkRed,
            Brushes.MidnightBlue
        };
        //dictionary
        // CYBERSECURITY RESPONSES
        private Dictionary<string, List<string>> cyberResponses =
            new Dictionary<string, List<string>>()
        {
            {
                "password",
                new List<string>()
                {
                    "Use strong passwords with symbols ",
                    "Never reuse passwords on different websites ",
                    "Enable two-factor authentication for extra protection 🛡"
                }
            },

            {
                "privacy",
                new List<string>()
                {
                    "Avoid sharing personal information publicly ",
                    "Review app permissions regularly ",
                    "Use privacy settings on social media "
                }
            },

            {
                "scams",
                new List<string>()
                {
                    "Never trust suspicious emails |",
                    "Scammers often create urgency to trick victims |",
                    "Verify links before clicking them |"
                }
            },

            {
                "phishing",
                new List<string>()
                {
                    "Avoid clicking suspicious links ",
                    "Check email addresses carefully ",
                    "Do not download unknown attachments ",
                    "Use antivirus software "
                }
            },
            {
                "malware",
                new List<string>()
                {
                    "Install trusted antivirus software.",
                    "Avoid downloading files from unknown websites.",
                    "Keep your operating system updated regularly."
                }
            },

            {
                "vpns",
                new List<string>()
                {
                    "A VPN helps protect your internet privacy.",
                    "Use trusted VPN services only.",
                    "VPNs are useful on public Wi-Fi networks."
                }
            },

            {
                "ransomware",
                new List<string>()
                {
                    "Always back up important files.",
                    "Do not open suspicious email attachments.",
                    "Ransomware can lock your files until money is paid."
                }
            },

            {
                "antivirus",
                new List<string>()
                {
                    "Keep your antivirus updated.",
                    "Run regular system scans.",
                    "Antivirus software helps detect threats early."
                }
            },

            {
                "hacking",
                new List<string>()
                {
                    "Enable two-factor authentication for better security.",
                    "Never reuse passwords across accounts.",
                    "Hackers often target weak passwords."
                }
            }
        };

        private string historyFile =
            "History/chat_history.txt";

        private bool nameStored = false;

        public MainWindow()
        {
            InitializeComponent();
            LoadQuiz();

            StartBackgroundAnimation();

            PlayGreeting();

            AddBotMessage("Hello,I am an Online Cyberbot Luna. What is your name?");
        }



        // The background color  
        private void StartBackgroundAnimation()
        {
            DispatcherTimer timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromSeconds(5);

            timer.Tick += (s, e) =>
            {
                MainGrid.Background = backgrounds[random.Next(backgrounds.Count)];
            };

            timer.Start();
        }

        // INTRO VOICE
        private void PlayGreeting()
        {
            try
            {
                //INTRO VOICES
                SoundPlayer player = new SoundPlayer("C:\\Users\\shlaps\\Pictures\\WpfApp1\\WpfApp1\\voice1.wav");

                player.Load();
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sound could not play: " + ex.Message);


            }
        }
        //SEND BUTTON
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string message =
                    UserInput.Text.Trim();

                if (string.IsNullOrWhiteSpace(message))
                {
                    AddBotMessage("Please type something to interact with Luna ");
                    return;
                }

                AddUserMessage(message);

                SaveMessage("USER", message);

                UserInput.Clear();

                string lower =
                    message.ToLower();
                if (quizRunning)
                {
                    if (message.Equals(quiz[currentQuestion].Answer,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        score++;

                        AddBotMessage(" CORRECT!\n" +
                                      quiz[currentQuestion].Explanation);
                    }
                    else
                    {
                        AddBotMessage(" INCORRECT.\nCorrect answer: " +
                                      quiz[currentQuestion].Answer +
                                      "\n" +
                                      quiz[currentQuestion].Explanation);
                    }

                    currentQuestion++;

                    if (currentQuestion >= quiz.Count)
                    {
                        quizRunning = false;
                        LogActivity("Quiz completed" +score);
                        if (score <= 5)
                        {
                            AddBotMessage($"Quiz Finished!\nYour score is {score}/{quiz.Count}" + " " + "Keep leanring to stay safe online ");
                        }
                        else if (score < 8)
                        {
                            AddBotMessage($"Quiz Finished!\nYour score is {score}/{quiz.Count}" + " " + "Great Job!! ");
                        }
                        else
                        {
                            AddBotMessage($"Quiz Finished!\nYour score is {score}/{quiz.Count}" + " " + "You are a cybersecurity pro!! ");
                        }
                        return;
                    }

                    AskQuestion();

                    return;
                }
                // ================= REMINDER =================
                if (waitingForReminder)
                {
                    // 3 days
                    if (lower.Contains("3 days") ||
                        lower.Contains("three days") ||
                        lower.Contains("in 3") ||
                        lower.Contains("remind me in 3"))
                    {
                        db.SetReminder(lastTaskId, DateTime.Now.AddDays(3));
                        LogActivity("Reminder set for task " + lastTaskId + " (3 days)");

                        AddBotMessage("Perfect! I'll remind you in 3 days.");

                        waitingForReminder = false;
                        return;
                    }

                    // 7 days
                    if (lower.Contains("7 days") ||
                        lower.Contains("seven days") ||
                        lower.Contains("one week") ||
                        lower.Contains("1 week") ||
                        lower.Contains("remind me next week"))
                    {
                        db.SetReminder(lastTaskId, DateTime.Now.AddDays(7));
                        LogActivity("Reminder set for task " + lastTaskId + " (7 days)");

                        AddBotMessage("Perfect! I'll remind you in 7 days.");

                        waitingForReminder = false;
                        return;
                    }

                    // No reminder
                    if (lower.Contains("no") ||
                        lower.Contains("don't") ||
                        lower.Contains("do not") ||
                        lower.Contains("not now") ||
                        lower.Contains("no thanks"))
                    {
                        AddBotMessage("No reminder has been set.");

                        waitingForReminder = false;
                        return;
                    }

                    AddBotMessage("Please say something like 'remind me in 3 days', 'one week', or 'no'.");
                    return;
                }
                if (lower.Contains("add task") ||
                     lower.Contains("create task") ||
                     lower.Contains("new task") ||
                     lower.Contains("set a reminder for") ||
                     lower.Contains("can you remind me to"))
                {
                    string taskTitle = message;

                    if (lower.Contains("add task"))
                    {
                        taskTitle = message.Substring("add task".Length).Trim();
                    }
                    else if (lower.Contains("create task"))
                    {
                        taskTitle = message.Substring("create task".Length).Trim();
                    }
                    else if (lower.Contains("new task"))
                    {
                        taskTitle = message.Substring("new task".Length).Trim();
                    }
                    else if (lower.Contains("set a reminder for"))
                    {
                        taskTitle = message.Substring("set a reminder for".Length).Trim();
                    }
                    else if (lower.Contains("can you remind me to"))
                    {
                        taskTitle = message.Substring("can you remind me to".Length).Trim();
                    }

                    if (string.IsNullOrWhiteSpace(taskTitle))
                    {
                        AddBotMessage("Please provide a task title.");
                        return;
                    }

                    db.AddTask(taskTitle, "TO ENSURE YOUR DATA IS PROTECTED");

                    LogActivity("Task added: " + taskTitle);

                    lastTaskId = db.GetLatestTaskId();
                    waitingForReminder = true;

                    AddBotMessage("Task added. Would you like a reminder? (e.g. remind me in 3 days)");

                    return;
                }
                if (lower.Contains("view task") ||
                    lower.Contains("show task") ||
                    lower.Contains("my tasks") ||
                    lower.Contains("list tasks"))
                {
                    string tasks = db.GetTasks();

                    AddBotMessage(tasks);

                    return;
                }
                if (lower.Contains("complete task") ||
                    lower.Contains("finish task") ||
                    lower.Contains("done task") ||
                    lower.Contains("mark task"))
                {
                    string idText = lower;

                    idText = idText.Replace("complete task", "");
                    idText = idText.Replace("finish task", "");
                    idText = idText.Replace("done task", "");
                    idText = idText.Replace("mark task", "");

                    idText = idText.Trim();

                    if (int.TryParse(idText, out int id))
                    {
                        if (db.CompleteTask(id))
                        {
                            LogActivity("Task completed: " + id);
                            AddBotMessage($"Task {id} marked as completed.");
                        }
                        else
                        {
                            AddBotMessage($"Task {id} was not found.");
                        }
                    }
                    else
                    {
                        AddBotMessage("Please enter a valid task ID.");
                    }
                    return;
                }
                if (lower.Contains("delete task") ||
                    lower.Contains("remove task") ||
                    lower.Contains("erase task"))
                {
                    string idText = lower;

                    idText = idText.Replace("delete task", "");
                    idText = idText.Replace("remove task", "");
                    idText = idText.Replace("erase task", "");

                    idText = idText.Trim();

                    if (int.TryParse(idText, out int id))
                    {
                        bool success = db.DeleteTask(id);

                        if (success)
                        {
                            LogActivity("Task deleted: " + id);
                            AddBotMessage($"Task {id} deleted.");
                        }
                        else
                        {
                            AddBotMessage($"Task {id} was not found.");
                        }
                    }
                    else
                    {
                        AddBotMessage("Please enter a valid task ID.");
                    }

                    return;
                }
                // SAVE FAVOURITE TOPIC USING "INTERESTED IN"
                if (lower.Contains("interested in"))
                {
                    string[] parts =
                        lower.Split(new string[] { "interested in" },
                        StringSplitOptions.None);

                    if (parts.Length > 1)
                    {
                        favouriteTopic = parts[1].Trim();

                        AddBotMessage("Great! I will remember that you are interested in "
                            + favouriteTopic);

                        return;
                    }
                }
                // EXIT COMMAND
                if (lower.Contains("bye") ||
                    lower.Contains("exit") ||
                    lower.Contains("goodbye"))
                {
                    AddBotMessage("Goodbye! Stay safe online ");

                    Application.Current.Shutdown();

                    return;
                }

                // STORE NAME
                if (!nameStored)
                {
                    memory.UserMemory["name"] = message;

                    nameStored = true;

                    await TypingAnimation();

                    AddBotMessage("Nice to meet you " + message);
                    AddBotMessage(message + " " + "You can ask me anything about cybersecurity type'START QUIZ'anytime to start a quiz to test your knowledge on cybersecurity");

                    return;
                }

                await TypingAnimation();


                if (lower.Contains("what is my name"))
                {
                    AddBotMessage("Your name is " + memory.UserMemory["name"]);
                }
               

                // Emotions
                // EMOTIONS + RANDOM CYBERSECURITY RESPONSE

                else if (lower.Contains("sad"))
                {
                    AddBotMessage("I'm sorry you're sad. " +
                        GetRandomCyberResponse());
                }

                else if (lower.Contains("happy"))
                {
                    AddBotMessage("That's amazing! " +
                        GetRandomCyberResponse());
                }

                else if (lower.Contains("worried"))
                {
                    AddBotMessage("Do not worry. " +
                        GetRandomCyberResponse());
                }

                else if (lower.Contains("frustrated"))
                {
                    AddBotMessage("I understand this can be frustrating. " +
                        GetRandomCyberResponse());
                }

                else if (lower.Contains("tell me more") ||
                         lower.Contains("another tip") ||
                         lower.Contains("explain more"))
                {
                    HandleFollowUp();


                }
                if (lower == "start quiz")
                {
                    LogActivity("Quiz started");
                    quizRunning = true;
                    currentQuestion = 0;
                    score = 0;

                    AskQuestion();

                    return;
                }
                if (lower.Contains("show activity log") ||
                    lower.Contains("activity log") ||
                    lower.Contains("what have you done for me"))
                {
                    if (activityLog.Count == 0)
                    {
                        AddBotMessage("No activities have been recorded yet.");
                    }
                    else
                    {
                        string log = "Recent Activity:\n\n";

                        foreach (string item in activityLog)
                        {
                            log += "• " + item + "\n";
                        }

                        AddBotMessage(log);
                    }

                    return;
                }

                // KEYWORD DETECTION
                else
                {
                    bool found = false;

                    foreach (var keyword in cyberResponses.Keys)
                    {
                        if (lower.Contains(keyword))
                        {
                            memory.LastTopic = keyword;

                            var responses = cyberResponses[keyword];

                            string randomResponse = responses[random.Next(responses.Count)];
                            if (!string.IsNullOrEmpty(favouriteTopic))
                            {
                                AddBotMessage("Since you are interested in " +
                                    favouriteTopic + ", " + randomResponse);
                            }
                            else
                            {
                                AddBotMessage(randomResponse);
                            }

                            found = true;

                            break;
                        }
                    }

                    if (!found)
                    {
                        AddBotMessage(" I am still learning about that topic.");
                    }
                }
            }

            catch (Exception ex)
            {
                AddBotMessage(" Error: " + ex.Message);
            }
        }

        // FOLLOW-UP SYSTEM
        private void HandleFollowUp()
        {
            if (memory.LastTopic != null && cyberResponses.ContainsKey(memory.LastTopic))
            {
                var responses = cyberResponses[memory.LastTopic];

                string randomResponse = responses[random.Next(responses.Count)];

                AddBotMessage(randomResponse);
            }
            else
            {
                AddBotMessage("Please ask about a topic first ");
            }
        }
        private string GetRandomCyberResponse()
        {
            List<string> allResponses =
                new List<string>();

            foreach (var topic in cyberResponses.Values)
            {
                allResponses.AddRange(topic);
            }

            return allResponses[random.Next(allResponses.Count)];
        }
        //QUIZ
        private void LoadQuiz()
        {
            quiz.Add(new QuizQuestion
            {
                Question = "(1)True or False:  it is safe to reuse the same strong password across multiple websites as long as you change it every year.",
                Options = new string[] { "True", "False" },
                Answer = "False",
                Explanation = "Reusing passwords is a major security risk because if one website is compromised, the hacker can use that same password to access all your other accounts. Changing it every year does not mitigate this risk.."
            });
            quiz.Add(new QuizQuestion
            {
                Question = "(2)True or False:  you should always back up your important files as a defense against ransomware.",
                Options = new string[] { "True", "False" },
                Answer = "True",
                Explanation = "Always back up important files. This is the best defense because if ransomware locks your files, you can restore them from a backup instead of paying the ransom."
            });
            quiz.Add(new QuizQuestion
            {
                Question = "(3)True or False:  Scammers often try to create a sense of calm and relaxation to get you to lower your guard.",
                Options = new string[] { "True", "False" },
                Answer = "False",
                Explanation = "Scammers often create urgency to trick victims.\" They create a sense of panic (e.g., Your bank account will be closed in 24 hours!) to force you to act quickly without thinking."
            });
            quiz.Add(new QuizQuestion
            {
                Question = "(4)True or False:   VPN's are recommend  to protect your privacy, especially on public Wi-Fi networks..",
                Options = new string[] { "True", "False" },
                Answer = "True",
                Explanation = "VPNs are useful on public Wi-Fi networks.This is because public Wi-Fi is often unsecured, and a VPN encrypts your data to protect it from interception."
            });
            quiz.Add(new QuizQuestion
            {
                Question = "(5)Which attack tricks users into giving personal information?\nA) Firewall\nB) Phishing\nC) VPN\nD) Antivirus",
                Options = new string[] { "A", "B", "C", "D" },
                Answer = "B",
                Explanation = "Phishing attacks trick users into revealing information."
            });
            quiz.Add(new QuizQuestion
            {
                Question = "(6)what is the primary purpose of an antivirus software?\nA) To create strong passwords for you.\nB) To help detect threats early and run system scans.\nC) To hide IP address\nD) To lock your files until a payment is made.",
                Options = new string[] { "A", "B", "C", "D" },
                Answer = "B",
                Explanation = "Phishing attacks trick users into revealing information."
            });
            quiz.Add(new QuizQuestion
            {
                Question = "(7)Phishing scams include all the following EXCEPT:\nA) Avoid clicking suspicious links.\nB) Check emails adresses carefully.\nC) Trust all attachments from people you know\nD) use Antivirus software",
                Options = new string[] { "A", "B", "C", "D" },
                Answer = "C",
                Explanation = "Do not download unknown attachments. A key tactic of phishing is to impersonate someone you know, so trusting all attachments from people you know is dangerous, as their email account might have been hacked."
            });
            quiz.Add(new QuizQuestion
            {
                Question = "(8)Hackers often target which of the following?\nA) Users who have two-factor athentication enabled.\nB) Weak passwords\nC) Users who update their operating system regularly. \nD) People who use trusted VPN services",
                Options = new string[] { "A", "B", "C", "D" },
                Answer = "B",
                Explanation = " Weak passwords are easy to guess or crack using automated tools, making them a primary target for hackers."
            });
            quiz.Add(new QuizQuestion
            {
                Question = "(9)What is the single most important piece of advice given regarding passwords?\nA) Use passwords with only numbers.\nB) Use the same password for all your banking apps\nC) Share your passwords with a trusted friend\nD) Enable two-factor athentication for extra protection",
                Options = new string[] { "A", "B", "C", "D" },
                Answer = "D",
                Explanation = "While using strong passwords is important, the advice explicitly states: \"Enable two-factor authentication for extra protection . 2FA adds a second layer of security (like a code sent to your phone), which is the single most effective step to secure your account, even if your password is compromised."
            });
            quiz.Add(new QuizQuestion
            {
                Question = "(10)Why is it important to verify links before clicking on them?\nA) To ensure the link is not too long.\nB) To check if the link has a .com domain.\nC) To avoid falling victim to scams and phishing attempts.\nD) To make sure the link loads faster.",
                Options = new string[] { "A", "B", "C", "D" },
                Answer = "C",
                Explanation = " Verifying links is a core defense against both scams and phishing.By hovering over a link to see its true destination before clicking, you can avoid being tricked into entering your credentials on a fake website."
            });

        }
        //ACTIVITY LOG
        private void LogActivity(string action)
        {
            activityLog.Add($"{DateTime.Now:HH:mm:ss} - {action}");

            //Keep only the last 50 actions
            if (activityLog.Count > 50)
                activityLog.RemoveAt(0);
        }
        private void AskQuestion()
        {
            AddBotMessage(quiz[currentQuestion].Question);
        }

        // BOT MESSAGE
        private void AddBotMessage(
            string message)
        {
            StackPanel stack = new StackPanel();

            TextBlock time = new TextBlock()
            {

                Text = DateTime.Now.ToString("HH:mm"),
                Foreground = Brushes.LightGray,
                FontSize = 11
            };

            Border border = new Border()
            {

                Background = Brushes.DarkSlateBlue,


                CornerRadius = new CornerRadius(15),


                Padding = new Thickness(12),


                Margin = new Thickness(5),

                MaxWidth = 420,

                HorizontalAlignment = HorizontalAlignment.Left
            };

            TextBlock text =
                new TextBlock()
                {
                    Text =
                    "Luna :" + message,

                    Foreground = Brushes.White,

                    FontSize = 16,

                    TextWrapping = TextWrapping.Wrap
                };

            border.Child = text;

            stack.Children.Add(time);
            stack.Children.Add(border);

            ChatPanel.Children.Add(stack);

            SaveMessage("BOT", message);
        }

        // USER MESSAGE
        private void AddUserMessage(string message)
        {
            StackPanel stack = new StackPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Right
            };

            TextBlock time =
                new TextBlock()
                {
                    Text = DateTime.Now.ToString("HH:mm"),

                    Foreground = Brushes.LightGray,

                    FontSize = 11,

                    HorizontalAlignment =
                    HorizontalAlignment.Right
                };

            Border border = new Border()
            {
                Background = Brushes.Teal,

                CornerRadius = new CornerRadius(15),

                Padding = new Thickness(12),

                Margin = new Thickness(5),

                MaxWidth = 420,

                HorizontalAlignment = HorizontalAlignment.Right
            };

            TextBlock text =
                new TextBlock()
                {
                    Text =
                    "You :" + message,

                    Foreground =
                    Brushes.White,

                    FontSize = 16,

                    TextWrapping =
                    TextWrapping.Wrap
                };

            border.Child = text;

            stack.Children.Add(time);
            stack.Children.Add(border);

            ChatPanel.Children.Add(stack);
        }

        // TYPING EFFECT Leave it as it
        private async Task TypingAnimation()
        {
            Border typingBorder = new Border()
            {
                Background =
                    Brushes.Gray,

                CornerRadius =
                    new CornerRadius(10),

                Padding =
                    new Thickness(10),

                Margin =
                    new Thickness(5),

                HorizontalAlignment =
                    HorizontalAlignment.Left
            };

            TextBlock typingText = new TextBlock()
            {
                Text = "Luna is Typing...",
                Foreground = Brushes.White
            };

            typingBorder.Child = typingText;

            ChatPanel.Children.Add(typingBorder);

            await Task.Delay(1200);

            ChatPanel.Children.Remove(typingBorder);
        }

        // SAVE CHAT
        private void SaveMessage(string sender, string message)
        {
            Directory.CreateDirectory("History");

            string line = $"{DateTime.Now} [{sender}] {message}";

            File.AppendAllText(historyFile, line + Environment.NewLine);
        }
    }
}
