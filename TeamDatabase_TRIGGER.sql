/*(20%) Create triggers to enforce the following constraints in your database:

a. Whenever a user provides a tip for a business, the “numTips” value for that business and the
“tipCount” value for the user should be updated.
*/

CREATE OR REPLACE FUNCTION Update_BussinessNumTips_and_UserTipCount() RETURNS trigger AS '
BEGIN
	UPDATE business
	SET numtips = newTotalTips
	FROM (
			SELECT businessID, COUNT(tip.businessID) as newTotalTips
			FROM tip
			GROUP BY tip.businessID
		 ) AS temp
	WHERE Business.businessID = temp.businessID;
	
	UPDATE Users
	SET tipcount = newTotalTipCount
	FROM (
			SELECT userID, Count(tip.userID) as newTotalTipCount
			FROM tip
			GROUP BY tip.userID
		 ) AS temp
	WHERE users.userID = temp.userID;
	return NULL;
END
' LANGUAGE plpgsql;  
    
CREATE TRIGGER BussinessNumTips_and_UserTipCount
AFTER INSERT ON tip
FOR EACH ROW
EXECUTE PROCEDURE Update_BussinessNumTips_and_UserTipCount();

/*
Test:
bussID : 5KheTjYPu1HcQzQFtm4_vw
userid:  jRyO2V1pA4CdVVqCIOPc1Q

select *
From tip
where tip.businessID = '5KheTjYPu1HcQzQFtm4_vw'

select *
from business
where business.businessID = '5KheTjYPu1HcQzQFtm4_vw'
--numtips = 39

select *
from users
where users.userid = 'jRyO2V1pA4CdVVqCIOPc1Q'
--tip count = 100


insert into tip Values('5KheTjYPu1HcQzQFtm4_vw','jRyO2V1pA4CdVVqCIOPc1Q','1988-08-22 08:22:22',22, 'hi');
--(businessID,userID, tipText, tipDate, likes)

delete from tip where tip.tiptext = 'hi'
*/
	


b. Similarly, when a customer checks-in a business, the “numCheckins” attribute value for that
business should be updated.

CREATE OR REPLACE FUNCTION Update_BusinessNumCheckins() RETURNS trigger AS '
BEGIN
	UPDATE business
	SET numCheckins = newTotalCheckins
	FROM (
			SELECT businessID, COUNT(CheckIN.businessID) as newTotalCheckins
			FROM CheckIn
			GROUP BY CheckIN.businessID
		 ) AS temp
	WHERE Business.businessID = temp.businessID;
	return NULL;
END
' LANGUAGE plpgsql;  
    
CREATE TRIGGER BusinessNumCheckins
AFTER INSERT ON checkin
FOR EACH ROW
EXECUTE PROCEDURE Update_BusinessNumCheckins();

/*
TESt:
select * 
from business
where businessID = 'jdJwziOX5FB2C2D-vESY0Q'
--businessID = 'jdJwziOX5FB2C2D-vESY0Q' Numcheckins= 19

select *
from checkin
where businessID = 'jdJwziOX5FB2C2D-vESY0Q'
-- 19 lines

insert into checkin Values('jdJwziOX5FB2C2D-vESY0Q',22,02,1922,'01:01:01',0)

delete from checkin where checkinyear='1922' and checkintime='01:01:01'

	UPDATE business
	SET numCheckins = newTotalCheckins
	FROM (
			SELECT businessID, COUNT(CheckIN.businessID) as newTotalCheckins
			FROM CheckIn
			GROUP BY CheckIN.businessID
		 ) AS temp
	WHERE Business.businessID = temp.businessID;
*/

/*c. When a user likes a tip, the “totalLikes” attribute value for the user who wrote that tip should be
updated.*/ 

CREATE OR REPLACE FUNCTION Update_UsersTotalLikes() RETURNS trigger AS '
BEGIN
	UPDATE Users
	SET totalLikes = newTotalLikes
	FROM (
			SELECT userID, SUM(tip.likes) as newTotalLikes
			FROM tip
			GROUP BY tip.userID
		 ) AS temp
	WHERE users.userID = temp.userID;
	return NULL;
END
' LANGUAGE plpgsql;  
    
CREATE TRIGGER UsersTotalLikes
AFTER UPDATE OF likes ON tip
FOR EACH ROW
EXECUTE PROCEDURE Update_UsersTotalLikes();

/* TEST:
select *
from users
where userID='OGbYrmAPIPFREaevHPQ1uA'
--total likes =140 

select *
from tip
where userID='OGbYrmAPIPFREaevHPQ1uA'

UPDATE tip
SET likes = 140
where userID='OGbYrmAPIPFREaevHPQ1uA'
*/



/*
Test your triggers with INSERT/UPDATE statements, i.e.,
o Add a tip for a business (insert to tips table) and make sure that the tip, business, and user
tables are updated correctly.
o Check-in to a business (insert to checkins table) and make sure that the checkin and
business tables are updated correctly.
o Like a tip (update the tips table) and make sure that the tips and user tables are updated
correctly.
Write your TRIGGER statements and test statements (INSERT, UPDATE statements) to a file named
““<your-team-name>_TRIGGER.sql”. 
*/