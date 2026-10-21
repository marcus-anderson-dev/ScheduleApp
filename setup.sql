-- USE client_schedule;

INSERT IGNORE INTO country (countryId, country, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES
 (1, 'US', NOW(), 'test', NOW(), 'test'),
 (2, 'UK', NOW(), 'test', NOW(), 'test');

INSERT IGNORE INTO city (cityId, city, countryId, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES
 (1, 'Phoenix',  1, NOW(), 'test', NOW(), 'test'),
 (2, 'New York', 1, NOW(), 'test', NOW(), 'test'),
 (3, 'London',   2, NOW(), 'test', NOW(), 'test');

INSERT IGNORE INTO address (addressId, address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES
 (1, '123 Main Street',   '', 1, '85001',   '555-111-1111', NOW(), 'test', NOW(), 'test'),
 (2, '456 Broadway',      '', 2, '10001',   '555-222-2222', NOW(), 'test', NOW(), 'test'),
 (3, '789 Oxford Street', '', 3, 'W1D 1BS', '555-333-3333', NOW(), 'test', NOW(), 'test');

INSERT IGNORE INTO customer (customerId, customerName, addressId, active, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES
 (1, 'John Smith',   1, 1, NOW(), 'test', NOW(), 'test'),
 (2, 'Maria Garcia', 2, 1, NOW(), 'test', NOW(), 'test'),
 (3, 'Alice Brown',  3, 1, NOW(), 'test', NOW(), 'test');

-- Login: username "test", password "test"
INSERT INTO `user` (userId, userName, password, active, createDate, createdBy, lastUpdate, lastUpdateBy)
SELECT (SELECT IFNULL(MAX(userId), 0) + 1 FROM `user` u2), 'test', 'test', 1, NOW(), 'test', NOW(), 'test'
FROM DUAL
WHERE NOT EXISTS (SELECT 1 FROM `user` u3 WHERE u3.userName = 'test');

INSERT IGNORE INTO appointment (appointmentId, customerId, userId, title, description, location, contact, `type`, url, `start`, `end`, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES
 (1, 1, (SELECT userId FROM `user` WHERE userName = 'test' LIMIT 1), 'Kickoff meeting',  'First meeting', 'Phoenix office', 'John Smith', 'Consultation', 'https://example.com/kickoff', '2026-11-02 14:00:00', '2026-11-02 15:00:00', NOW(), 'test', NOW(), 'test'),
 (2, 2, (SELECT userId FROM `user` WHERE userName = 'test' LIMIT 1), 'Progress review',  'Quarterly review', 'New York office', 'Maria Garcia', 'Review', '', '2026-11-03 15:00:00', '2026-11-03 16:00:00', NOW(), 'test', NOW(), 'test'),
 (3, 3, (SELECT userId FROM `user` WHERE userName = 'test' LIMIT 1), 'Follow-up call',   'Phone follow-up', 'London office', 'Alice Brown', 'Follow-up', '', '2026-12-01 16:00:00', '2026-12-01 17:00:00', NOW(), 'test', NOW(), 'test'),
 (4, 1, (SELECT userId FROM `user` WHERE userName = 'test' LIMIT 1), 'Planning session', 'Plan next phase', 'Phoenix office', 'John Smith', 'Planning', '', '2026-12-02 17:00:00', '2026-12-02 18:00:00', NOW(), 'test', NOW(), 'test');
