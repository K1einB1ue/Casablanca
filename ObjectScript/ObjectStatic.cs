using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Object_Detail
{
    ObjectOnGroundBase Object_Handler { get; }
}
public interface ObjectOnTheGround
{
    Object_Values_Handler Object_Values_Handler { get; }
    object Object { get; }
}
public interface Object_Values_Handler
{
    IEnumerable<DialogMachine> DialogMachines { get; }
    bool BeSelected { get; set; }
    void BeDmged(float DMG);
    void BeFixed(float FIX);
}

public interface ObjectOnGroundBase
{
    void Refresh();
    void CollisionEnterBase(Collision collision);
    void CollisionStayBase(Collision collision);
    void CollisionExitBase(Collision collision);
    void TriggerEnterBase(Collider other);
    void TriggerStayBase(Collider other);
    void TriggerExitBase(Collider other);
    void UpdateBase();
}
public abstract class ObjectBase : ObjectOnGroundBase
{
    public ObjectOnGroundBase Object_Handler => this; 
    void ObjectOnGroundBase.CollisionEnterBase(Collision collision) {
        this.CollisionEnter(collision);
        this.CollisionEnter(collision.collider);
        if (collision.collider.TryGetComponent<ObjectOnTheGround>(out var objectOnTheGround)) {
            this.CollisionEnter(objectOnTheGround);
            if (objectOnTheGround.Object is Item) {
                this.CollisionEnter((Item)objectOnTheGround.Object);
            }
            if (objectOnTheGround.Object is Character) {
                this.CollisionEnter((Character)objectOnTheGround.Object);
            }
        }
    }

    void ObjectOnGroundBase.CollisionStayBase(Collision collision) {
        this.CollisionStay(collision);
        this.CollisionStay(collision.collider);
        if (collision.collider.TryGetComponent<ObjectOnTheGround>(out var objectOnTheGround)) {
            this.CollisionStay(objectOnTheGround);
            if (objectOnTheGround.Object is Item) {
                this.CollisionStay((Item)objectOnTheGround.Object);
            }
            if (objectOnTheGround.Object is Character) {
                this.CollisionStay((Character)objectOnTheGround.Object);
            }
        }
    }
    void ObjectOnGroundBase.CollisionExitBase(Collision collision) {
        this.CollisionExit(collision);
        this.CollisionExit(collision.collider);
        if (collision.collider.TryGetComponent<ObjectOnTheGround>(out var objectOnTheGround)) {
            this.CollisionExit(objectOnTheGround);
            if (objectOnTheGround.Object is Item) {
                this.CollisionExit((Item)objectOnTheGround.Object);
            }
            if (objectOnTheGround.Object is Character) {
                this.CollisionExit((Character)objectOnTheGround.Object);
            }
        }
    }

    void ObjectOnGroundBase.TriggerEnterBase(Collider other) {
        this.TriggerEnter(other);
        if (other.TryGetComponent<ObjectOnTheGround>(out var objectOnTheGround)) {
            this.TriggerEnter(objectOnTheGround);
            if (objectOnTheGround.Object is Item) {
                this.TriggerEnter((Item)objectOnTheGround.Object);
            }
            if (objectOnTheGround.Object is Character) {
                this.TriggerEnter((Character)objectOnTheGround.Object);
            }
        }
    }
    void ObjectOnGroundBase.TriggerStayBase(Collider other) {
        this.TriggerStay(other);
        if (other.TryGetComponent<ObjectOnTheGround>(out var objectOnTheGround)) {
            this.TriggerStay(objectOnTheGround);
            if (objectOnTheGround.Object is Item) {
                this.TriggerStay((Item)objectOnTheGround.Object);
            }
            if (objectOnTheGround.Object is Character) {
                this.TriggerStay((Character)objectOnTheGround.Object);
            }
        }
    }
    void ObjectOnGroundBase.TriggerExitBase(Collider other) {
        this.TriggerExit(other);
        if (other.TryGetComponent<ObjectOnTheGround>(out var objectOnTheGround)) {
            this.TriggerExit(objectOnTheGround);
            if (objectOnTheGround.Object is Item) {
                this.TriggerExit((Item)objectOnTheGround.Object);
            }
            if (objectOnTheGround.Object is Character) {
                this.TriggerExit((Character)objectOnTheGround.Object);
            }
        }
    }
    public virtual void CollisionEnter(Collider collider) { }
    public virtual void CollisionEnter(Collision collision) { }
    public virtual void CollisionEnter(Item item) { }
    public virtual void CollisionEnter(Character character) { }
    public virtual void CollisionEnter(ObjectOnTheGround obj) { }

    public virtual void CollisionStay(Collider collider) { }
    public virtual void CollisionStay(Collision collision) { }
    public virtual void CollisionStay(Item item) { }
    public virtual void CollisionStay(Character character) { }
    public virtual void CollisionStay(ObjectOnTheGround obj) { }

    public virtual void CollisionExit(Collider collider) { }
    public virtual void CollisionExit(Collision collision) { }
    public virtual void CollisionExit(Item item) { }
    public virtual void CollisionExit(Character character) { }
    public virtual void CollisionExit(ObjectOnTheGround obj) { }


    public virtual void TriggerEnter(Collider other) { }
    public virtual void TriggerEnter(Item item) { }
    public virtual void TriggerEnter(Character character) { }
    public virtual void TriggerEnter(ObjectOnTheGround obj) { }

    public virtual void TriggerStay(Collider other) { }
    public virtual void TriggerStay(Item item) { }
    public virtual void TriggerStay(Character character) { }
    public virtual void TriggerStay(ObjectOnTheGround obj) { }

    public virtual void TriggerExit(Collider other) { }
    public virtual void TriggerExit(Item item) { }
    public virtual void TriggerExit(Character character) { }
    public virtual void TriggerExit(ObjectOnTheGround obj) { }

    public abstract void UpdateBase();

    public virtual void Refresh() { }
}