﻿if OBJECT_ID('CardsDecks') is not null
drop table CardsDecks

if object_id('UsersCards') is not null
drop table UsersCards

if object_id('DiscussionResults') is not null
drop table DiscussionResults

if object_id('UsersInRooms') is not null
drop table UsersInRooms

if object_id('Rooms') is not null
drop table Rooms

if object_id('Users') is not null
drop table Users

if OBJECT_ID('Cards')is not null
drop table Cards

if object_id('Decks') is not null
drop table Decks

CREATE TABLE [dbo].[Cards] (
    [Id]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [Image] NVARCHAR (255) NULL,
    [Name]  NVARCHAR (255) NULL,
    [Owner] NVARCHAR (255) NULL,
    [Value] INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


CREATE TABLE [dbo].[Decks] (
    [Id]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]  NVARCHAR (255) NULL,
    [Owner] NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[CardsDecks] (
    [Card_id] BIGINT NOT NULL,
    [Deck_id] BIGINT NOT NULL,
    PRIMARY KEY CLUSTERED ([Deck_id] ASC, [Card_id] ASC),
    FOREIGN KEY ([Deck_id]) REFERENCES [dbo].[Decks] ([Id]) on delete cascade,
    FOREIGN KEY ([Card_id]) REFERENCES [dbo].[Cards] ([Id])
);

CREATE TABLE [dbo].[Users] (
    [Id]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);



CREATE TABLE [dbo].[Rooms] (
    [Id]            BIGINT         IDENTITY (1, 1) NOT NULL,
    [Password]      NVARCHAR (255) NULL,
    [Name]          NVARCHAR (255) NULL,
    [TimerDuration] BIGINT         NULL,
    [Owner_id]      BIGINT         NULL,
    [Deck_id]       BIGINT         NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FKE456B23EC47788C4] FOREIGN KEY ([Owner_id]) REFERENCES [dbo].[Users] ([Id]),
    CONSTRAINT [FKE456B23EB7C37A9] FOREIGN KEY ([Deck_id]) REFERENCES [dbo].[Decks] ([Id])
);

CREATE TABLE [dbo].[DiscussionResults] (
    [Id]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [Beginning] DATETIME2 (7)  NULL,
    [Ending]    DATETIME2 (7)  NULL,
    [Theme]     NVARCHAR (255) NULL,
    [Resume]    NVARCHAR (255) NULL,
    [Room_id]   BIGINT         NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FKDDB49C6CFC2E7751] FOREIGN KEY ([Room_id]) REFERENCES [dbo].[Rooms] ([Id])
);

CREATE TABLE [dbo].[UsersCards] (
    [Id]                  BIGINT IDENTITY (1, 1) NOT NULL,
    [User_id]             BIGINT NULL,
    [Card_id]             BIGINT NULL,
    [DiscussionResult_id] BIGINT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FKCA4CBCFF44752581] FOREIGN KEY ([Card_id]) REFERENCES [dbo].[Cards] ([Id]),
    CONSTRAINT [FKCA4CBCFFA66FDB98] FOREIGN KEY ([DiscussionResult_id]) REFERENCES [dbo].[DiscussionResults] ([Id]),
    CONSTRAINT [FKCA4CBCFFDFEF210] FOREIGN KEY ([User_id]) REFERENCES [dbo].[Users] ([Id])
);

CREATE TABLE [dbo].[UsersInRooms] (
    [Room_id] BIGINT NOT NULL,
    [User_id] BIGINT NOT NULL,
    PRIMARY KEY CLUSTERED ([User_id] ASC, [Room_id] ASC),
    FOREIGN KEY ([User_id]) REFERENCES [dbo].[Users] ([Id]),
    FOREIGN KEY ([Room_id]) REFERENCES [dbo].[Rooms] ([Id]) on delete cascade
);


insert into Cards values
('one.jpg', 'one', null, 1),
('two.jpg', 'two', null, 2),
('three.jpg', 'three', null, 3),
('five.jpg', 'five', null, 5),
('eight.jpg', 'eight', null, 8),
('thirdteen.jpg', 'thirdteen', null, 13),
('coffee.jpg', 'coffee', null, null),
('twenty.jpg', 'twenty', 'valera', 20),
('forthy.jpg', 'forthy', 'valera', 40)


insert into Decks values
('fib', null ),
('somethingdeck', 'valera' )


insert into CardsDecks values
(1, 1),
(2, 1),
(3, 1),
(4, 1),
(5, 1),
(6, 1),
(7, 1),
(8, 2),
(9, 2)

insert into Users values ('valera'), ('Borya')

insert into Rooms values 
('','Just Room', null, 1, 1),
('1','Room of pain', 256, 2, 1),
('','Room with timer', 666666, 2, 1)

insert into UsersInRooms values 
(1, 1),
(2, 2)

insert into DiscussionResults values
(GETDATE(), GETDATE(), 'test theme', 'test resume', 1)

insert into UsersCards values
(1, 2, 1),
(2, 3, 1)