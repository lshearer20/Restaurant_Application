CREATE TABLE Users (
    userID VARCHAR(30),
    averageStars FLOAT CHECK (averageStars <=5.0 AND averageStars >=0.0),
    fans INT,
    name VARCHAR(128),
    cool INT,
    funny INT,
    useful INT,
    tipCount INT,
    totalLikes INT,
    yelpingSince DATE,
    userLatitude FLOAT,
    userLongitude FLOAT,
    PRIMARY KEY (userID)
);


CREATE TABLE Business (
    businessID VARCHAR(30),
    name VARCHAR(128),
    state VARCHAR(2),
    city VARCHAR(128),
    address VARCHAR(128),
    zipcode INT,
    stars FLOAT CHECK (stars <=5.0 AND stars >=0.0),
    isOpen INT,
    latitude FLOAT,
    longitude FLOAT,
    numCheckins INT,
    numTips INT,
    PRIMARY KEY (businessID)
);

CREATE TABLE Tip (    
    businessID VARCHAR(30) NOT NULL,
    userID VARCHAR(30) NOT NULL,    
    tipDate TimeStamp,
    likes INT,
    tipText VARCHAR,
    PRIMARY KEY (businessID,userID,tipDate),
    FOREIGN KEY (businessID) REFERENCES Business(businessID),
    FOREIGN KEY (userID) REFERENCES Users(userID)
);

CREATE TABLE friends (
    userID VARCHAR(30),
    friendID VARCHAR(30),
    PRIMARY KEY (userID,friendID),
    FOREIGN KEY (userID) REFERENCES Users(userID),
    FOREIGN KEY (friendID) REFERENCES Users(userID)

);

CREATE TABLE Categories (
    businessID VARCHAR(30),
    categoriesName VARCHAR(128),
    PRIMARY KEY (businessID,categoriesName),
    FOREIGN KEY (businessID) REFERENCES Business(businessID)
);

CREATE TABLE Attributes (
    businessID VARCHAR(30),
    attributeName VARCHAR(128),
    attributeValue VARCHAR(30),
    PRIMARY KEY (businessID,attributeName),
    FOREIGN KEY (businessID) REFERENCES Business(businessID)
);

CREATE TABLE Hours (
    businessID VARCHAR(30),
    hoursDay VARCHAR(10),
    hoursOpen TIME,
    hoursClose TIME,
    PRIMARY KEY (businessID,hoursDay),
    FOREIGN KEY (businessID) REFERENCES Business(businessID)
);

CREATE TABLE CheckIn (
    businessID VARCHAR(30),
    checkinDay VARCHAR(10),
    checkinMonth VARCHAR(10),
    checkinYear VARCHAR(10),
    checkinTime TIME,
    checkinCount INT,
    PRIMARY KEY (businessID,checkinDay,checkinMonth,checkinYear,checkinTime),
    FOREIGN KEY (businessID) REFERENCES Business(businessID)
);
