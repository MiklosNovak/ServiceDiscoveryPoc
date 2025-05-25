from fastapi import FastAPI, HTTPException
import asyncio, os, socket
from pydantic import BaseModel
from serviceregistryclient import ServiceRegistryClient

# Hardcoded inventory based on your SKUs with random initial quantities
inventory = {
    "IP14P": 5,   # iPhone 14 Pro
    "MB23A": 3,   # MacBook Air 2023
    "PS5C": 7,    # PlayStation 5 Console
    "GC20": 25,   # Gift Card $20
    "HD1T": 10,   # 1TB Hard Drive
    "TV55": 2,    # 55-inch TV
    "WMN1": 6,    # Women's Nike Shoes
    "TSH1": 12,   # T-Shirt Size M
    "BAGX": 8,    # Travel Bag
    "HDPH": 15,   # Headphones
    "CBL1": 30,   # USB-C Cable
    "KEYM": 4,    # Mechanical Keyboard
    "MSEW": 9,    # Wireless Mouse
    "CHG1": 20,   # Phone Charger
    "FANX": 11    # Portable Fan
}

app = FastAPI()     


sidecar_url = os.environ.get('SideCarUrl')
client = ServiceRegistryClient(sidecar_url)

hostname = socket.gethostname()
port = 8000

print(f"Registering service at {hostname}:{port} with sidecar URL {sidecar_url}")

registration_request = {
"name": "InventoryServiceApi",
"address": hostname,
"port": port
}

client.register_service(registration_request)


class ReservationRequest(BaseModel):
    product_id: str
    quantity: int

@app.post("/inventory/reserve")
def reserve_item(request: ReservationRequest):
    if request.product_id not in inventory:
        raise HTTPException(status_code=404, detail="Product not found")
    
    if inventory[request.product_id] >= request.quantity:
        inventory[request.product_id] -= request.quantity
        return {"success": True, "message": "Reservation successful"}
    else:
        return {"success": False, "message": "Insufficient stock"}

@app.get("/health")
def health_check():
    return {"status": "OK"}

@app.get("/inventory")
def list_inventory():
    return inventory
    
