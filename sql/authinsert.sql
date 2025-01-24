-- insert roles
Insert into [dbo].[AspNetRoles]
(Id, Name, NormalizedName)
 Values 
(1, 'Admin', 'ADMIN'),
(2, 'Staff', 'STAFF'),
(3, 'Member', 'MEMBER');

-- then create a user with the admin role
INSERT INTO [dbo].[AspNetUsers] (
    Id, 
	FirstName,
	LastName,
    UserName, 
    NormalizedUserName, 
    Email, 
    NormalizedEmail, 
    EmailConfirmed, 
    PasswordHash, 
	City,
	Country,
	Address,
	ActiveMember,
	DateOfBirth,
	RegistrationDate,
	PhoneNumber,
	PhoneNumberConfirmed,
	TwoFactorEnabled,
    SecurityStamp, 
    ConcurrencyStamp,
	LockoutEnabled,
	AccessFailedCount
) 
VALUES (
    '24cbb067-2787-4d1f-af7d-55c8fadf5092', 
	'Admin',
	'User',
    'adminuser', 
    'ADMINUSER', 
    'admin@example.com', 
    'ADMIN@EXAMPLE.COM', 
    1, 
    'Test1234!',
	'Admin City',
	'Admin Country', 
	'Admin Address',
	1,
	'2000-10-10',
	'2024-10-10',
	'+33712345678',
	0,
	0,
    NEWID(), 
    NEWID(),
	0,
	0
)


-- set the user to the admin role
insert into [dbo].[AspNetUserRoles]
(UserId, RoleId)
Values
('24cbb067-2787-4d1f-af7d-55c8fadf5092', 1)

-- add staff if needed for setup