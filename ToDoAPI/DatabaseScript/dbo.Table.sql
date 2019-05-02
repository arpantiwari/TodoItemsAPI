--Create the table
CREATE TABLE ToDoItems(ID INTEGER PRIMARY KEY AUTOINCREMENT, UserName TEXT NOT NULL, TaskName TEXT NOT NULL, Priority TEXT, DueDate DATETIME, Status TEXT, isChecked TINYINT DEFAULT 0);
--Insert sample data for API
INSERT INTO ToDoItems(TaskName, UserName, Priority, DueDate, Status, isChecked) values ('Buy groceries', 'Admin', 'Low',  '2018-11-29', 'New', 1);
INSERT INTO ToDoItems(TaskName, UserName, Priority, DueDate, Status) values ('Pay bills','Dev', 'High', '2018-11-30', 'In Progress');
--Create Users table
CREATE TABLE Users ( UserID INTEGER PRIMARY KEY AUTOINCREMENT, UserName TEXT, Password TEXT );
--Insert dummy users
--Note: The passwords would ideally be saved as encrypted salt values in production using an encryption algorithm instead of plaintext
INSERT INTO Users ( UserName, Password) values ('Admin', 'pass');
INSERT INTO Users ( UserName, Password) values ('Dev', 'devpass');