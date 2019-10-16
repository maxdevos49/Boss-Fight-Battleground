use BFB;

CREATE TABLE BFB_Login (
	LoginId INT AUTO_INCREMENT NOT NULL,
    UserId INT NOT NULL,
    Token VARCHAR(50),
    IsActive BOOLEAN,
    InsertedOn DATE,
    InsertedBy DATE,
    UpdatedOn DATE,
    UpdatedBy DATE,
    PRIMARY KEY ( LoginId ),
    FOREIGN KEY ( UserId ) REFERENCES BFB_User(UserId)
);

CREATE TABLE BFB_Role (
	RoleId INT AUTO_INCREMENT NOT NULL,
    Name VARCHAR(30) NOT NULL,
    Description VARCHAR(100) NOT NULL,
    IsActive BOOLEAN,
    InsertedOn DATE,
    InsertedBy DATE,
    UpdatedOn DATE,
    UpdatedBy DATE,
    PRIMARY KEY ( RoleId )
);

CREATE TABLE BFB_UserRole (
	UserRoleId INT AUTO_INCREMENT NOT NULL,
    UserId INT NOT NULL,
    RoleId INT NOT NULL,
    IsActive BOOLEAN,
    InsertedOn DATE,
    InsertedBy DATE,
    UpdatedOn DATE,
    UpdatedBy DATE,
	PRIMARY KEY ( UserRoleId ),
    FOREIGN KEY ( UserId ) REFERENCES BFB_User(UserId),
    FOREIGN KEY ( RoleId ) REFERENCES BFB_Role(RoleId)
);

CREATE TABLE BFB_UserStats (
	UserStatId INT AUTO_INCREMENT NOT NULL,
    UserId INT NOT NULL,
    GamesPlated INT NOT NULL,
    IsActive BOOLEAN,
    InsertedBy DATE,
    InsertedOn DATE,
    UpdatedBy DATE,
    UpdatedOn DATE,
    PRIMARY KEY ( UserStatId ),
    FOREIGN KEY ( UserId ) REFERENCES BFB_User(UserId)
);

CREATE TABLE BFB_Game (
	GameId INT AUTO_INCREMENT NOT NULL,
    PlayerKills INT NOT NULL,
    MonsterKills INT NOT NULL,
    BossKills INT NOT NULL,
    InsertedOn DATE,
    InsertedBy DATE,
    UpdatedOn DATE,
    UpdatedBy DATE,
    PRIMARY KEY ( GameId )
);

CREATE TABLE BFB_GameMembers (
	GameMemberId INT AUTO_INCREMENT NOT NULL,
    GameId INT NOT NULL,
    UserId INT NOT NULL,
    BossKills INT NOT NULL,
    MonsterKills INT NOT NULL,
    PlayerKills INT NOT NULL,
    TimeAsBoss INT NOT NULL,
    TimeAsMonster INT NOT NULL,
    TimeAsPlayer INT NOT NULL,
    IsActive BOOLEAN,
    InsertedBy DATE,
    InsertedOn DATE,
    UpdatedOn DATE,
    UpdatedBy DATE,
    PRIMARY KEY ( GameMemberId ),
    FOREIGN KEY ( GameId ) REFERENCES BFB_Game(GameId),
    FOREIGN KEY ( UserId ) REFERENCES BFB_User(UserId)
);