import requests
from typing import Any

class ServiceRegistryClient:
    def __init__(self, sidecar_url: str):
        if not sidecar_url.endswith('/'):
            sidecar_url += '/'
        self.base_url = sidecar_url

    def register_service(self, request_data: Any) -> None:
        url = f"{self.base_url}services"
        headers = {'Content-Type': 'application/json'}
        response = requests.post(url, json=request_data, headers=headers)

        if not response.ok:
            raise Exception(f"Failed to register service: {response.text}")
