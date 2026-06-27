
-- ==========================================
-- Cyberbot Tasks Database Creation Script
-- Author: THUTO SEHLAPELO
-- ==========================================

-- Create the database
CREATE DATABASE IF NOT EXISTS cyberbot_tasks;

-- Use the database
USE cyberbot_tasks;

-- Remove the table if it already exists
DROP TABLE IF EXISTS tasks;

-- Create the tasks table
CREATE TABLE tasks
(
    tasks_id INT AUTO_INCREMENT PRIMARY KEY,

    title VARCHAR(255) NOT NULL,

    description TEXT,

    reminder_date DATETIME NULL,

    is_completed TINYINT(1) NOT NULL DEFAULT 0
);



-- Verify the table
SELECT * FROM tasks;