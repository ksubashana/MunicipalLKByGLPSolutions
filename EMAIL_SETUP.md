# Email Notification Setup for Inspector Assignments

This document explains how to configure and test the email notification feature for inspector assignments.

## Configuration

### SMTP Settings
Add the following configuration to your `appsettings.json` file:

```json
{
  "Email": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": "587", 
    "SmtpUsername": "your-email@gmail.com",
    "SmtpPassword": "your-app-password",
    "FromEmail": "noreply@municipality.lk",
    "FromName": "Municipal System"
  }
}
```

### Gmail Configuration (Example)
1. Enable 2-factor authentication on your Gmail account
2. Generate an App Password for the application
3. Use the App Password in the `SmtpPassword` field
4. Set `SmtpHost` to `smtp.gmail.com` and `SmtpPort` to `587`

### Local Development
For local development, you can use a local SMTP server like MailHog:
```json
{
  "Email": {
    "SmtpHost": "localhost",
    "SmtpPort": "1025",
    "SmtpUsername": "",
    "SmtpPassword": "",
    "FromEmail": "dev@municipality.lk",
    "FromName": "Municipal System - Development"
  }
}
```

## How It Works

1. When an inspector is assigned via the `AssignInspector.razor` page
2. The `CreateAssignmentCommandHandler` processes the assignment
3. After successfully creating the assignment, it:
   - Retrieves the inspector's contact information using the `AssignedTo` GUID
   - Sends an email with inspection details including:
     - Application number
     - Scheduled date and time
     - Remarks (if any)

## Email Template

The email sent to inspectors includes:
- Inspector's name
- Application number
- Scheduled date and time
- Any remarks from the assignment
- Professional greeting and signature

## Error Handling

- Email failures do not prevent assignment creation
- All email errors are logged but do not throw exceptions
- If the inspector doesn't have an email address, no email is sent

## Testing

To test the email functionality:

1. Configure valid SMTP settings in `appsettings.json`
2. Create an inspector user with a valid email address
3. Assign an inspection through the BuildingAndPlanningCreate.razor page
4. Check the inspector's email inbox for the notification
5. Monitor application logs for email sending status

## Troubleshooting

- Check SMTP configuration if emails are not being sent
- Verify inspector contact has a valid email address
- Review application logs for email service errors
- For Gmail, ensure 2FA is enabled and App Password is used