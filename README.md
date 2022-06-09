# WSEI_2022_PO PROJEKT ZALICZENIOWY
## SNAKE GAME
### *INTRODUCTION*
Simple game which allow you to control snake real-time. The goal is collecting as many points as you can. Every collected red dot is worth one point, increases length of snake and speeds it up. After death your score is saved to local database and thanks to it you can compare to other players in highscores.
For the needs of project the admin panel has been added with username and password filled. It allows the user to delete records from database.

### **CRUD**
If player with specific nickname cannot be found in database it will be created with their first score.
If player with specific nickname can be found in database their score will be updated.
Highscores list displays all users with their scores and places with descending order by score.
Admin panel allows to delete players.

### *FRAMEWORK*
.NET 5.0
### *UI*
WPF/XAML
Application contains 3 windows:
  - MainWindow - the window you need to enter your nickname to start game
  - HighscoresWindow - displays list of players and their place
  - AdminWindow - allows you to log in and get access to deleting players (username and password is filled by default for the needs of project username = *admin* password = *password*)
### *DATABASE*
SQLite v3.0
Contains two tables:
  - PlayerDataModel - stores all players and their IDs
  - Scores - stores scores for all players with foreign key referenced to their ID
### *ORM*
Dapper
