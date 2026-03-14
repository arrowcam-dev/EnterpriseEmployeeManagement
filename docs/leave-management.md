# Leave Management Module

## EnterpriseEmployeeManagement

---

# 1. Overview

The **Leave Management module** allows employees to request time off and enables managers or HR administrators to review and approve those requests.

This module ensures that:

- Employees can submit leave requests
- Leave balances are tracked
- Managers approve or reject leave requests
- Leave usage is auditable
- Reports can be generated

This document describes the **database design, relationships, and business workflow** used in the Leave Management system.

---

# 2. Core Features

The Leave Management module provides the following features:

- Define leave types (Annual Leave, Sick Leave, etc.)
- Track leave balances for employees
- Submit leave requests
- Manager approval workflow
- Leave request history tracking
- Leave usage reports
- Notifications for leave actions

---

# 3. Database Tables

The Leave Management module uses the following tables:

| Table | Purpose |
|------|--------|
| Employees | Employee information |
| LeaveTypes | Defines available leave categories |
| LeaveBalances | Tracks employee leave entitlement |
| LeaveRequests | Stores leave requests |
| LeaveRequestHistory | Tracks request status changes |

Optional extension:

| Table | Purpose |
|------|--------|
| LeaveNotifications | Stores notification records |

---

# 4. Database Schema

## 4.1 Employees

This table already exists in the system and represents employees.

| Column | Type | Description |
|------|------|-------------|
| Id | Guid / int | Employee identifier |
| FirstName | string | First name |
| LastName | string | Last name |
| DepartmentId | int | Department reference |
| ManagerId | int | Direct manager |
| Status | string | Active / Inactive |
| CreatedAt | datetime | Record creation time |

Purpose:

- Identify employees requesting leave
- Identify the manager responsible for approval

---

## 4.2 LeaveTypes

Defines different types of leave available in the organization.

| Column | Type | Description |
|------|------|-------------|
| Id | int | Leave type identifier |
| Name | string | Leave type name |
| Description | string | Description |
| DefaultDaysPerYear | int | Default leave entitlement |
| IsPaid | bool | Paid leave indicator |
| IsActive | bool | Active status |
| CreatedAt | datetime | Creation timestamp |

Example Leave Types:
```
Annual Leave
Sick Leave
Unpaid Leave
Maternity Leave
Paternity Leave
```

---

## 4.3 LeaveBalances

Tracks leave entitlement and usage for employees.

| Column | Type | Description |
|------|------|-------------|
| Id | int | Record identifier |
| EmployeeId | int | Employee reference |
| LeaveTypeId | int | Leave type reference |
| TotalDays | int | Allowed leave days |
| UsedDays | int | Days already used |
| RemainingDays | int | Remaining leave days |
| Year | int | Leave year |

Example:
```
Employee: John Smith
LeaveType: Annual Leave
TotalDays: 18
UsedDays: 5
RemainingDays: 13
```

Purpose:

- Prevent employees from exceeding leave entitlement
- Track leave usage by year

---

## 4.4 LeaveRequests

Stores employee leave requests.

| Column | Type | Description |
|------|------|-------------|
| Id | int | Request identifier |
| EmployeeId | int | Employee requesting leave |
| LeaveTypeId | int | Leave type |
| StartDate | date | Leave start date |
| EndDate | date | Leave end date |
| TotalDays | int | Calculated leave duration |
| Reason | string | Leave reason |
| Status | string | Request status |
| CreatedAt | datetime | Submission timestamp |
| ApprovedBy | int | Manager ID |
| ApprovedAt | datetime | Approval timestamp |

Possible Status Values:
```
Pending
Approved
Rejected
Cancelled
```

---

## 4.5 LeaveRequestHistory

Tracks the lifecycle of leave requests.

| Column | Type | Description |
|------|------|-------------|
| Id | int | History record ID |
| LeaveRequestId | int | Associated request |
| Status | string | Status change |
| ChangedBy | int | User who changed status |
| ChangedAt | datetime | Timestamp |
| Comment | string | Optional comment |

Example History Records:
```
Submitted by Employee
Approved by Manager
Rejected by Manager
Cancelled by Employee
```


Purpose:

- Maintain audit trail
- Track request workflow

---

# 5. Table Relationships
```
Employee
│
├── LeaveBalances
│
└── LeaveRequests
│
├── LeaveTypes
│
└── LeaveRequestHistory
```

Relationship Summary:

| Relationship | Type |
|--------------|------|
Employee → LeaveRequests | One-to-Many |
LeaveTypes → LeaveRequests | One-to-Many |
Employee → LeaveBalances | One-to-Many |
LeaveRequests → LeaveRequestHistory | One-to-Many |

---

# 6. Business Logic Workflow

## Step 1 — Employee Submits Leave Request

Employee fills out a leave request form with:
```
Leave Type
Start Date
End Date
Reason
```


System performs validations:

- Validate date range
- Calculate requested leave days
- Check available leave balance

Example:
```
Annual Leave Balance = 12 days
Requested Leave = 5 days
Result = Allowed
```

---

## Step 2 — Leave Request Created

System inserts a record into the `LeaveRequests` table.

Example:
```
EmployeeId: 101
LeaveType: Annual Leave
StartDate: 2026-04-01
EndDate: 2026-04-05
TotalDays: 5
Status: Pending
```

System also creates a history record:
```
LeaveRequestHistory
Status = Submitted
```

---

## Step 3 — Manager Notification

Manager receives notification about the leave request.

Example message:
```
Employee
Leave Type
Requested Days
Remaining Leave Balance
Reason
```
Manager actions:
```
Approve
Reject
```

---

## Step 5 — Approval Logic

If the request is **approved**:
```
LeaveRequests.Status = Approved
ApprovedBy = ManagerId
ApprovedAt = CurrentDate
```
Leave balance is updated:
```
UsedDays += RequestedDays
RemainingDays -= RequestedDays
```

---

## Step 6 — History Tracking

A history record is created:
LeaveRequestHistory
```
Status: Approved
ChangedBy: Manager
ChangedAt: Timestamp
```

---

## Step 7 — Employee Notification

Employee receives notification:
```
Your leave request has been approved
```
or
```
Your leave request has been rejected
```

---

# 7. Leave Balance Calculation

Example:
```
TotalDays = 18
UsedDays = 6
RemainingDays = 12
```
Employee requests:
```
RequestedDays = 5
```
Validation rule:
```
RequestedDays <= RemainingDays
```
If the condition fails:
```
Reject request
```

---

# 8. Suggested Service Architecture (.NET)

Recommended services:
```
LeaveService
LeaveBalanceService
LeaveApprovalService
LeaveNotificationService
```
Typical request flow:
```
LeaveController
│
LeaveService
│
LeaveBalanceService
│
Repository
│
Database
```

---

# 9. UI Pages

Recommended UI pages for Leave Management:
```
Employee Leave Request Page
My Leave Requests
Manager Approval Page
Leave Management Dashboard
Leave Reports
```

---

# 10. Reporting

Possible reports include:
```
Leave usage by employee
Leave usage by department
Monthly leave summary
Annual leave usage
```

Future enhancement:
```
Export to Excel
Export to PDF
```

---

# 11. Future Enhancements

Advanced enterprise HR systems may include:
```
Half-day leave support
Leave carry-forward
Holiday calendar integration
Automatic leave accrual
Multi-level approval workflows
Department-level leave reports
```

---

# 12. Summary

The Leave Management module includes:

- 5 core database tables
- Approval workflow
- Leave balance tracking
- Request history auditing
- Notification support
- Reporting capabilities

This design ensures the system is **scalable, maintainable, and suitable for enterprise HR applications**.





