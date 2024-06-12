# Secure Password Manager
A secure way to manage passwords on a local machine.

## Windows Installation


### Install Windows Terminal
- It can be found in microsoft store for free if you search ***Windows Terminal***


### Run SPM.exe
- type `keygen` and copy the genrated hash
- type `q` to quit the program

### Configure .my.cnf file
-  Go to scripts folder
-  Open **.my.cnf** file
-  Paste your key from `keygen` command
-  Enter a database name that will be used later
-  Save the file and close it
-  Open Powershell where .my.cnf is located
    - This can be done by `shift + rclick` within the folder, select ***open Powershell window here***
-  Encrypt the file
    - `cipher /e .\.my.cnf` - this will encrypt the file

### Install MySQL Community 8.0.37 or higher
- https://dev.mysql.com/downloads/installer/
- Select Developer Install
- Continue to make a root password
- Don't install example databases
- Uncheck open workbench
- Keep shell checked

#### Open MysqlShell
(make sure you are in JS mode type \js)
- Create a new user
    `\connect root@localhost` -password was created on install.
    `\sql`
    `CREATE USER 'newuser'@'%' IDENTIFIED BY 'newpassword';`
    `GRANT ALL PRIVILEGES ON *.* TO 'newuser'@'%';`
    `FLUSH PRIVILEGES;`
    `\disconnect`
    `\connect newuser@localhost` -enter new password

#### Create a new Database
- `CREATE DATABASE newdatabase;`
- `SHOW DATABASES;`   - it will show up in list if working
- `CREATE TABLE `services` (
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
    CONSTRAINT `userID` FOREIGN KEY (`userID`) REFERENCES `users`       (`userID`) ON DELETE CASCADE) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;`
- `CREATE TABLE `services` (
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
        CONSTRAINT `userID` FOREIGN KEY (`userID`) REFERENCES `users` (`userID`) ON DELETE CASCADE) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;`
- `SHOW TABLES` - services and users should show up

#### Configure .my.cnf
        - go to /scripts/.my.cnf
        - edit in your database
        - edit where you want to save database backup files
        - save the file and close it
#### Configuration mylogin.cnf
- Go to mysql installation files, usually found in: 
    `C:\Program Files\MySQL\MySQL Server 8.0\bin\`
- Open Terminal in the bin directory and paste with your database username:
    `./mysql_config_editor set --login-path=client --host=localhost --user={your_username} --password`
- file is saved in: "C:\Users\{your_username}\AppData\Roaming\MySQL\.mylogin.cnf"

## Add Environment Variables
- Add `MYSQL_COMMANDS` with value `path\to\mysqlserver\bin\`
- Add `POWERSHELL` with value `C:\WINDOWS\System32\WindowsPowerShell\v1.0\powershell.exe`


- Run this command to create a secure configuration file for database password
    - `./mysql_config_editor set --login-path=client --host=localhost --user=your_username --password`
- it will prompt you to enter a user password to login, (usually stored in ROAMING\APPDATA)

msqldump, msql : Usually found in ("C:\Program Files\MySQL\MySQL Server 8.0\bin\mysqldump.exe")
C:\Program Files\MySQL\MySQL Server 8.0\bin\mysql.exe
