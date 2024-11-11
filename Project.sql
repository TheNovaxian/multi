IF db_id('College1en') IS NULL 
    CREATE DATABASE College1en;
GO

USE College1en;

IF OBJECT_ID('Enrollments', 'U') IS NOT NULL DROP TABLE Enrollments;
IF OBJECT_ID('Students', 'U') IS NOT NULL DROP TABLE Students;
IF OBJECT_ID('Courses', 'U') IS NOT NULL DROP TABLE Courses;
IF OBJECT_ID('Programs', 'U') IS NOT NULL DROP TABLE Programs;

CREATE TABLE Programs (
    ProgId VARCHAR(5) NOT NULL PRIMARY KEY, 
    ProgName VARCHAR(50) NOT NULL
   
);


CREATE TABLE Courses(
    CId VARCHAR(7) NOT NULL PRIMARY KEY, 
    CName VARCHAR(50) NOT NULL,
    ProgId VARCHAR(5) NOT NULL,
    FOREIGN KEY (ProgId) REFERENCES Programs(ProgId)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);


CREATE TABLE Students(
    StId VARCHAR(10) NOT NULL PRIMARY KEY, 
    StName VARCHAR(50) NOT NULL,
    ProgId VARCHAR(5) NOT NULL,
    FOREIGN KEY (ProgId) REFERENCES Programs(ProgId) 
	  ON UPDATE CASCADE
      ON DELETE NO ACTION
);


CREATE TABLE Enrollments(
    StId VARCHAR(10) NOT NULL,
    CId VARCHAR(7) NOT NULL,
    FinalGrade INT NULL,
    PRIMARY KEY (StId, CId),
    FOREIGN KEY (StId) REFERENCES Students(StId)
	  ON UPDATE CASCADE
	  ON DELETE CASCADE,
    FOREIGN KEY (CId) REFERENCES Courses(CId)
	  ON UPDATE NO ACTION
	  ON DELETE NO ACTION,
  
);

GO


INSERT INTO Programs (ProgId, ProgName) 
VALUES ('P0001', 'Computer Science'),
       ('P0002', 'Electrical Engineering'),
       ('P0003', 'Mechanical Engineering'),
       ('P0004', 'Chemical Engineering'),
       ('P0005', 'Civil Engineering');

INSERT INTO Courses (CId, CName, ProgId) 
VALUES ('C000001', 'Introduction to Computer Science', 'P0001'),
       ('C000002', 'Introduction to Electrical Engineering', 'P0002'),
       ('C000003', 'Introduction to Mechanical Engineering', 'P0003'),
       ('C000004', 'Introduction to Chemical Engineering', 'P0004'),
       ('C000005', 'Introduction to Civil Engineering', 'P0005');


INSERT INTO Students (StId, StName, ProgId) 
VALUES ('S000000001', 'John', 'P0001'),
       ('S000000002', 'Mary', 'P0003'),
       ('S000000003', 'Jane', 'P0004'),
       ('S000000004', 'Mark', 'P0005'),
       ('S000000005', 'Jill', 'P0002');


INSERT INTO Enrollments (StId, CId, FinalGrade) 
VALUES ('S000000001', 'C000001', NULL),
       ('S000000002', 'C000002', NULL),
       ('S000000003', 'C000003', NULL),
       ('S000000004', 'C000004', NULL),
       ('S000000005', 'C000005', NULL);

GO
