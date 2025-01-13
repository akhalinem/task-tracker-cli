# TaskTracker CLI

A simple command-line task management application built with .NET 9.0 that allows you to create, update, delete, and track tasks with different statuses.

## Features

- Add new tasks
- Update existing tasks
- Remove tasks
- Mark task status (Todo, In Progress, Done)
- List all tasks
- Filter tasks by status
- Persistent storage using JSON file

## Prerequisites

- .NET 9.0 SDK
- A terminal/command prompt

## Installation

1. Clone the repository:
    ```bash
    git clone [repository-url]
    ```
2. Navigate to the project directory:
    ```bash
    cd task-tracker
    ```
3. Restore dependencies:
    ```bash
    dotnet restore
    ```

## Usage

1. Build the project:
    ```bash
    dotnet build
    ```
2. Run the application:
    ```bash
    dotnet run -- [command]
    ```

### Commands

- `add <description>`: Add a new task
- `update <id> <description>`: Update an existing task
- `remove <id>`: Remove a task
- `mark <id> <status>`: Mark task status (todo/in-progress/done)
- `list`: List all tasks
- `list <status>`: List tasks by status (todo/in-progress/done)

### Examples

- Add a new task:
    ```bash
    dotnet run -- add "Buy groceries"
    ```
- Update an existing task:
    ```bash
    dotnet run -- update <task-id> "Buy groceries and cleaning supplies"
    ```
- Remove a task:
    ```bash
    dotnet run -- remove <task-id>
    ```
- Mark a task as in-progress:
    ```bash
    dotnet run -- mark <task-id> in-progress
    ```
- List all tasks:
    ```bash
    dotnet run -- list
    ```

## Running Tests

To run the tests, use the following command:
```bash
dotnet test
```