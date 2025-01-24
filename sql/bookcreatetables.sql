- Books table
CREATE TABLE Books (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ISBN NVARCHAR(13) NOT NULL UNIQUE,
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    PublicationDate DATE NOT NULL,
    Publisher NVARCHAR(100) NOT NULL,
    Language NVARCHAR(50) NOT NULL,
    TotalPages INT NOT NULL,
    AvailableCopies INT NOT NULL DEFAULT 0,
    AddedDate DATETIME NOT NULL DEFAULT GETDATE(),
    LastUpdated DATETIME,
    isRemoved BIT NOT NULL DEFAULT 0
    
);

-- Authors table
CREATE TABLE Authors (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Biography NVARCHAR(MAX)
);

-- BookAuthors junction table
CREATE TABLE BookAuthors (
    BookId INT NOT NULL,
    AuthorId INT NOT NULL,
    PRIMARY KEY (BookId, AuthorId),
    CONSTRAINT FK_BookAuthors_Books FOREIGN KEY (BookId) REFERENCES Books(Id),
    CONSTRAINT FK_BookAuthors_Authors FOREIGN KEY (AuthorId) REFERENCES Authors(Id)
);

-- Genres table
CREATE TABLE Genres (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(255),
);

-- BookGenres junction table
CREATE TABLE BookGenres (
    BookId INT NOT NULL,
    GenreId INT NOT NULL,
    PRIMARY KEY (BookId, GenreId),
    CONSTRAINT FK_BookGenres_Books FOREIGN KEY (BookId) REFERENCES Books(Id),
    CONSTRAINT FK_BookGenres_Genres FOREIGN KEY (GenreId) REFERENCES Genres(Id)

);

-- BookLoans table
CREATE TABLE BookLoans (
    Id INT PRIMARY KEY IDENTITY(1,1),
    BookId INT NOT NULL,
    UserId NVARCHAR(255) NOT NULL,
    BorrowDate DATETIME NOT NULL DEFAULT GETDATE(),
    DueDate DATETIME NOT NULL,
    ReturnDate DATETIME,
    Status NVARCHAR(20) NOT NULL CHECK (Status IN ('Active', 'Returned', 'Overdue')),
    Notes NVARCHAR(MAX),
    CONSTRAINT FK_BookLoans_Books FOREIGN KEY (BookId) REFERENCES Books(Id),
);

-- Fines table
CREATE TABLE Fines (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId NVARCHAR(255) NOT NULL,
    BookLoanId INT NOT NULL,
    Amount DECIMAL(10,2) NOT NULL CHECK (Amount > 0),
    Reason NVARCHAR(255) NOT NULL,
    IssuedDate DATETIME NOT NULL DEFAULT GETDATE(),
    DueDate DATETIME NOT NULL,
    PaidDate DATETIME,
    Status NVARCHAR(20) NOT NULL CHECK (Status IN ('Pending', 'Paid', 'Waived')),
    CONSTRAINT FK_Fines_BookLoans FOREIGN KEY (BookLoanId) REFERENCES BookLoans(Id)

);


-- SQL INSERT statements for Genre table
INSERT INTO [dbo].[Genres](Name, Description)
VALUES 
    ('SciFi', 'Science Fiction genre'),
    ('Mystery', 'Mysterious and suspenseful stories'),
    ('Romance', 'Love and relationship-focused narratives'),
    ('Horror', 'Frightening and scary stories'),
    ('Thriller', 'Exciting and suspenseful narratives'),
    ('Comedy', 'Humorous and lighthearted stories'),
    ('Drama', 'Serious and emotional narratives'),
    ('Action', 'High-energy and exciting stories'),
    ('Adventure', 'Exploratory and exciting narratives'),
    ('Biography', 'Life stories of real people'),
    ('Autobiography', 'Self-written life stories'),
    ('History', 'Historical accounts and narratives'),
    ('Science', 'Scientific and educational content'),
    ('Math', 'Mathematical and numerical content'),
    ('Philosophy', 'Philosophical thoughts and discussions'),
    ('Religion', 'Religious and spiritual content'),
    ('SelfHelp', 'Personal development and improvement'),
    ('Health', 'Medical and wellness-related content'),
    ('Fitness', 'Exercise and physical health content'),
    ('Cooking', 'Culinary and recipe-focused content'),
    ('Travel', 'Exploration and journey-related stories'),
    ('Guide', 'Instructional and guidance content'),
    ('Children', 'Content targeted at children'),
    ('YoungAdult', 'Content for young adult readers'),
    ('Adult', 'Content for adult readers'),
    ('Other', 'Miscellaneous or uncategorized content');

    -- Mock INSERT statements for Author table
INSERT INTO [dbo].[Authors] (FirstName, LastName, Biography)
VALUES 
    ('Stephen', 'King', 'Renowned horror and suspense novelist known for works like The Shining and It.'),
    ('Jane', 'Austen', 'Celebrated English novelist famous for romantic fiction set in early 19th-century England.'),
    ('Ernest', 'Hemingway', 'Influential American writer known for his concise writing style and novels like The Old Man and the Sea.'),
    ('Agatha', 'Christie', 'Best-selling mystery writer famous for detective novels featuring Hercule Poirot and Miss Marple.'),
    ('Neil', 'Gaiman', 'Acclaimed author of fantasy novels, comics, and short stories.'),
    ('Margaret', 'Atwood', 'Prominent Canadian author known for speculative fiction and dystopian novels.'),
    ('George', 'Orwell', 'British writer and journalist famous for political novels like 1984 and Animal Farm.'),
    ('Haruki', 'Murakami', 'Internationally renowned Japanese novelist known for surreal and metaphysical fiction.'),
    ('Maya', 'Angelou', 'Celebrated poet, memoirist, and civil rights activist.'),
    ('Gabriel', 'García Márquez', 'Colombian novelist known for magical realism and Nobel Prize in Literature.');


CREATE INDEX IX_Books_ISBN ON Books(ISBN);
CREATE INDEX IX_Books_Title ON Books(Title);
CREATE INDEX IX_BookLoans_Status ON BookLoans(Status);
CREATE INDEX IX_BookLoans_DueDate ON BookLoans(DueDate);
CREATE INDEX IX_Fines_Status ON Fines(Status);




