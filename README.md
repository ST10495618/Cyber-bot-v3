# Cyber-bot-v3
# Luna Cyberbot – Cybersecurity Awareness Chatbot

## Overview

Luna Cyberbot is a Windows Presentation Foundation (WPF) chatbot developed in C# as part of a Portfolio of Evidence (POE). The application promotes cybersecurity awareness by providing users with security advice, managing cybersecurity-related tasks, setting reminders, and testing users' knowledge through an interactive quiz.

The chatbot also uses a MySQL database to store user tasks and reminders.

---
Project Evolution
Version 1 – Console Application

The first version of Luna Cyberbot was developed as a console application.

Features Included
Text-based chatbot interface
Cybersecurity awareness tips
Keyword detection
User name memory
Conversation history
Random cybersecurity responses
Follow-up responses
Basic Natural Language Processing (NLP)

This version focused on creating the chatbot's core functionality before introducing a graphical interface.
## Version 2 – Graphical User Interface (GUI)

The second version transformed the chatbot from a console application into a Windows Presentation Foundation (WPF) desktop application.

### New Features
- Modern graphical user interface
- Chat bubble interface
- Animated background colours
- User profile image
- Voice greeting on startup
- Improved chatbot interaction
- Enhanced user experience
- Better organisation of chatbot responses

---

## Version 3 – Intelligent Cybersecurity Assistant

The third version extends the chatbot into a more intelligent assistant by integrating database functionality, task management, quizzes, reminders, and activity tracking.

### New Features
- MySQL database integration
- Task management system
- Add, view, complete and delete tasks
- Reminder system
- Activity log
- Cybersecurity quiz with scoring and feedback
- Improved Natural Language Processing (NLP)
- Multiple command recognition (e.g. "add task", "create task", "new task")
- Better memory of user interactions
- Persistent task storage using MySQL
## Features

### Cybersecurity Chatbot

* Responds to cybersecurity-related questions.
* Provides tips on:

  * Password Security
  * Privacy
  * Phishing
  * Scams
  * Malware
  * VPNs
  * Ransomware
  * Antivirus
  * Hacking
* Displays responses using a friendly chat interface.

### Task Management

Users can:

* Add new tasks
* View saved tasks
* Mark tasks as completed
* Delete tasks
* Store tasks in a MySQL database

### Reminder System

The chatbot allows users to:

* Set reminders for tasks
* Choose reminder periods such as 3 days or 7 days
* Save reminder dates in the database

### Natural Language Processing (NLP)

The chatbot recognises different ways users ask for the same action.

Examples:

* "Add task"
* "Create task"
* "New task"
* "Show tasks"
* "View my tasks"
* "Complete task"
* "Delete task"

### Cybersecurity Quiz

Includes an interactive multiple-choice quiz that:

* Tests cybersecurity knowledge
* Provides explanations after each answer
* Calculates the user's score
* Records quiz activity

### Activity Log

The chatbot records important events including:

* Tasks added
* Tasks completed
* Tasks deleted
* Reminders created
* Quiz started
* Quiz completed

Users can display the activity log by requesting it through the chatbot.

---

## Technologies Used

* C#
* WPF (Windows Presentation Foundation)
* .NET Framework
* MySQL
* MySQL Connector/NET
* Visual Studio
* MySQL Workbench

---

## Database

The application uses a MySQL database named:

```
cyberbot_tasks
```

Main table:

```
tasks
```

Columns:

* tasks_id
* title
* description
* reminder_date
* is_completed

A SQL creation script is included with this repository.

---

## Installation

### Requirements

* Visual Studio 2022
* .NET Framework
* MySQL Server 8.x
* MySQL Workbench
* MySQL Connector/NET

### Steps

1. Clone the repository.

2. Import the provided SQL file:

```
cyberbot_tasks.sql
```

3. Open the solution in Visual Studio.

4. Update the connection string in **BotDatabase.cs** if your MySQL username or password differs.

Example:

```csharp
server=localhost;
database=cyberbot_tasks;
uid=root;
pwd=your_password;
```

5. Build and run the project.

---

## Project Structure

```
WpfApp1/
│
├── MainWindow.xaml
├── MainWindow.xaml.cs
├── BotDatabase.cs
├── ChatMemory.cs
├── QuizQuestion.cs
├── History/
├── Images/
├── Sounds/
└── cyberbot_tasks.sql
```

---

## Example Commands

```
Add task Review passwords
```

```
View tasks
```

```
Complete task 1
```

```
Delete task 2
```

```
Start quiz
```

```
Show activity log
```

```
Tell me about phishing
```

---

## Future Improvements

* Voice recognition
* AI-powered responses
* Email reminder notifications
* User authentication
* Cloud database support
* Additional cybersecurity topics

---

## Author

Developed by **[THUTO SEHLAPELO]**

Portfolio of Evidence (POE)

Cybersecurity Awareness Chatbot
