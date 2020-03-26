﻿namespace Authentication.EventStore.Models
{
    public enum EventAction
    {
        UserRegistered,
        EmailUniqueValidationFailed,
        EmailVerified,
        EmailChanged,
        UserDeleted
    }
}
