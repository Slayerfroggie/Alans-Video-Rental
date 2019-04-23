SET IDENTITY_INSERT RentalItem ON
INSERT INTO RentalItem (RentalItemId, RentalId, MovieId) VALUES (1,1,1);
INSERT INTO RentalItem (RentalItemId, RentalId, MovieId) VALUES (2,1,2);
INSERT INTO RentalItem (RentalItemId, RentalId, MovieId) VALUES (3,2,3);
INSERT INTO RentalItem (RentalItemId, RentalId, MovieId) VALUES (4,3,1);
INSERT INTO RentalItem (RentalItemId, RentalId, MovieId) VALUES (5,3,2);
INSERT INTO RentalItem (RentalItemId, RentalId, MovieId) VALUES (6,3,3);
SET IDENTITY_INSERT RentalItem OFF
Go