
/*“numCheckins” value for a business should be updated to the count of all check-in counts for that
business. */

UPDATE business
SET numCheckins = newTotalCheckins
FROM (
		SELECT businessID, COUNT(CheckIN.businessID) as newTotalCheckins
		FROM CheckIn
		GROUP BY CheckIN.businessID
	 ) AS temp
WHERE Business.businessID = temp.businessID

/*Similarly, “numTips” should be updated to the number of tips provided for that business.
*/

UPDATE business
SET numtips = newTotalTips
FROM (
		SELECT businessID, COUNT(tip.businessID) as newTotalTips
		FROM tip
		GROUP BY tip.businessID
	 ) AS temp
WHERE Business.businessID = temp.businessID

/*^^^^you should query the “checkins” and “tips” tables to calculate these values.^^^^
*/

/*“totalLikes” value for a user should be updated to the sum of all likes for the user’s tips. 
*/


UPDATE Users
SET totalLikes = newTotalLikes
FROM (
		SELECT userID, SUM(tip.likes) as newTotalLikes
		FROM tip
		GROUP BY tip.userID
	 ) AS temp
WHERE users.userID = temp.userID



/*And “tipCount” should be updated to the number of tips that the user provided for various businesses.
*/

UPDATE Users
SET tipcount = newTotalTipCount
FROM (
		SELECT userID, Count(tip.userID) as newTotalTipCount
		FROM tip
		GROUP BY tip.userID
	 ) AS temp
WHERE users.userID = temp.userID

/*^^^You should query the “Tips” table to calculate these values.^^^
In grading, points will be deducted if you don’t update these values correctly. 
*/