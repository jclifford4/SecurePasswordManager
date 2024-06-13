# Secure Password Manager
## A secure way to manage passwords on a local machine.

## Windows Installation
***Reminder*** : Any code snipped surrounded by `{}` will need your information

### Install Windows Terminal
- It can be found in microsoft store for free if you search ***Windows Terminal***

---

### Run SPM.exe
- type `keygen` and copy the genrated hash.
- type `q` to quit the program.

---

### Configure .my.cnf file
-  Go to scripts directory
-  Open ***.my.cnf*** file
-  Paste your key from `keygen` command
-  Enter a database name that will be used later
-  Save the file and close it
-  Open Powershell where .my.cnf is located
    - This can be done by `shift + rclick` within the folder, select ***open Powershell window here***
-  Encrypt the file
```shell
cipher /e .\.my.cnf
``` 

---

### Install MySQL Community 8.0.37 or higher
- https://dev.mysql.com/downloads/installer/
- Select Developer Install
- Continue to make a root password
- Don't install example databases
- Uncheck open workbench
- Keep shell checked

---

### Open MysqlShell
- make sure you are in JS mode type `\js`

##### Connect with root
- *password was created on install*
```sql
\connect root@localhost
``` 
##### Change to sql mode
```sql
\sql
```
##### Create a new user 
```sql
CREATE USER '{newuser}'@'%' IDENTIFIED BY 'newpassword';
```
##### Grant and Flush privileges:
```sql
GRANT ALL PRIVILEGES ON *.* TO '{newuser}'@'%';
```

```sql
FLUSH PRIVILEGES;
```

```sql
\disconnect
```

##### Connect with your user:
```sql
\connect {newuser}@localhost
```

---

#### Create a new Database
```sql
CREATE DATABASE {newdatabase};
```

#### Display new database
```sql
SHOW DATABASES;
```   

#### Create users table
```sql
CREATE TABLE `users` (
  `userID` int NOT NULL AUTO_INCREMENT,
  `userName` varchar(25) NOT NULL,
  `passwordHash` varchar(84) NOT NULL,
  `creationDate` datetime DEFAULT NULL,
  `guid` varchar(36) NOT NULL,
  PRIMARY KEY (`userID`),
  UNIQUE KEY `userID_UNIQUE` (`userID`),
  UNIQUE KEY `userName_UNIQUE` (`userName`),
  UNIQUE KEY `guid_UNIQUE` (`guid`)
) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
```
#### Create services table
```sql
CREATE TABLE `services` (
    `passID` int NOT NULL AUTO_INCREMENT,
    `userID` int NOT NULL,
    `service` varchar(25) NOT NULL,
    `encryptedPassword` varchar(128) NOT NULL,
    `guid` varchar(36) NOT NULL,
    `creationDate` datetime NOT NULL,
    PRIMARY KEY (`passID`),
    UNIQUE KEY `passID_UNIQUE` (`passID`),
    UNIQUE KEY `guid_UNIQUE` (`guid`),
    KEY `userID` (`userID`),
    CONSTRAINT `userID` FOREIGN KEY (`userID`) REFERENCES `users` (`userID`) ON DELETE CASCADE) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
```
#### Verify tables wer created

```sql
SHOW TABLES;
```

---

### Configure .my.cnf 
- go to ***/scripts/.my.cnf***
- edit in your database name
- edit where you want to save database backup files
- save the file and close it

--- 

### Configuration mylogin.cnf
- Go to mysql installation files, usually found in: 
```
C:\Program Files\MySQL\MySQL Server 8.0\bin\
```
- Open Terminal in the bin directory and paste with your database username:
```shell
./mysql_config_editor set --login-path=client --host=localhost --user={your_username} --password
```
- file is saved in: ***C:\Users\{your_username}\AppData\Roaming\MySQL\.mylogin.cnf***

### Add Environment Variables
- Add `MYSQL_COMMANDS` with value `{path\to\mysqlserver\bin\}`
- Add `POWERSHELL` with value `C:\WINDOWS\System32\WindowsPowerShell\v1.0\powershell.exe`
