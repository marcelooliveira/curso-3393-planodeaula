using System;
using System.Collections.Generic;

// Base class for all entities
public abstract class BaseEntity
{
    public int Id { get; set; }

    protected BaseEntity(int id)
    {
        Id = id;
    }
}
