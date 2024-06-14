# Secure Password Manager
A secure way to manage passwords on a local machine.

---
# About
<details>
<summary>General Information</summary>

### Passwords
***Plaintext passwords are never saved.***

**Account Passwords** (*master password*) : are hashed and salted then are stored to the localdatabase only accessible by that user.

**Service Passwords** : are symmetrically encrypted and are stored in the local database. They can only be decrpyted by confirming the master password of the user account.

</details>

<details>

<summary>Storage, Hashing, & Encryption</summary>

<br />

***This program uses MySQL server and MySQL Shell to store hashed and encrypted sensitive data only.***

- ***Hashed + salted*** master passwords are stored for each user. These are needed to **unlock** service passwords.
  - 100k iterations using PBKDF2
  - **PBKDF2** applies a pseudorandom function like HMAC to the input password and applies a salt iteratively.
  - ```markdown
  [PBKDF2](https://en.wikipedia.org/wiki/PBKDF2#:~:text=PBKDF2%20applies%20a%20pseudorandom%20function,cryptographic%20key%20in%20subsequent%20operations target="_blank"), [HMAC](https://en.wikipedia.org/wiki/HMAC target="_blank")
  ```
- ***Encrpted*** service passwords are stored and can only be ***decrypted*** thorugh use of correct user master password.
  - User generates a *32byte* key converted to *base64 string* that acts as the symmetrical key for Encrypting & Decrypting.
  - User can generate a new key if needed, all service passwords that used the old key will need to be updated.
  - If old key is removed or lost, all service passwords will unable to be decrypted to their original form.
</details>

---
## Installation
<details>

<br />

<summary>Windows Guide</summary>

<br />

***Reminder*** : Any code snipped surrounded by `{}` will need your information.

### Download the latest [Release](https://github.com/jclifford4/SecurePasswordManager/releases){:target="_blank"}.
### Unzip files into desired location.

---

### Install Windows Terminal
- It can be found in microsoft store for free if you search ***Windows Terminal***.
- Open Terminal and set it as default Terminal in settings.

---

### Run SPM.exe
- type `keygen` and copy the genrated hash.
- type `q` to quit the program.

---

### Configure .my.cnf file
-  Go to the ***scripts*** directory.
-  Open ***.my.cnf*** file.
-  Paste your key from `keygen` command after `Encryption=`.
-  Enter a database name that will be used later.
-  Save the file and close it.
-  Open Powershell where .my.cnf is located.
    - This can be done by `shift + rclick` within the folder, select ***open Powershell window here***.
-  Encrypt the file : 
```shell
cipher /e .\.my.cnf
``` 

---

### Install MySQL Community 8.0.37 or higher
- https://dev.mysql.com/downloads/installer/
- Select Full Install
- Continue to make a root password.
- This should install `mysqlserver` and `mysqlshell`.

---

### Open MysqlShell :
- make sure you are in JS mode type `\js`

##### Connect with root :
- *password was created on install*
```sql
\connect root@localhost
``` 
##### Change to sql mode :
```sql
\sql
```
##### Create a new user :
```sql
CREATE USER '{your_username}'@'%' IDENTIFIED BY '{your_password}';
```
##### Grant and Flush privileges :
```sql
GRANT ALL PRIVILEGES ON *.* TO '{your_username}'@'%';
```
```sql
FLUSH PRIVILEGES;
```
```sql
\disconnect
```

##### Connect with your user :
```sql
\connect {your_username}@localhost
```

---

#### Create a new Database :
```sql
CREATE DATABASE {your_database_name};
```

#### Verify new database exists : 
```sql
SHOW DATABASES;
```   

#### Connect to new database : 
```sql
USE {your_database_name};
```   

#### Create users table :
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
#### Create services table :
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
#### Verify tables were created :
```sql
SHOW TABLES;
```

---

### Configure .my.cnf :
- Open ***/scripts/.my.cnf***
- Edit in your database name if you haven't already.
- Edit in a location for database backups.
  - Preferably in /backups folder.
- Edit where you want to save database backup files.
- Save the file and close it.

--- 

### Configuration mylogin.cnf :
- Go to mysql installation files, usually found in : 
```
C:\Program Files\MySQL\MySQL Server 8.0\bin\
```
- Open Terminal in the bin directory and paste with your database username :
```shell
./mysql_config_editor set --login-path=client --host=localhost --user={your_username} --password
```
- file is saved in: ***C:\Users\{your_username}\AppData\Roaming\MySQL\.mylogin.cnf***

### Add Environment Variables :
- Add `MYSQL_COMMANDS` with path `{path\to\mysqlserver\bin\}`
- Add `POWERSHELL` with path `C:\WINDOWS\System32\WindowsPowerShell\v1.0\powershell.exe`

</details>
