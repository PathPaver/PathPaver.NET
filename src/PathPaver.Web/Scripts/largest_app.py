from pymongo import MongoClient
import pandas as pd
import matplotlib.pyplot as plt
import numpy as np
import json
import ipywidgets as widgets
from ipywidgets import interact
from dotenv import dotenv_values

config = dotenv_values(".env")

client = MongoClient(config["DB_URI"])

try:
    client.admin.command('ping')
    print("Connection réussie!")
    db = client.PATH_PAVER_DEV
    collection = db.rents
    df = pd.DataFrame(list(collection.find()))
except Exception as e:
    print(e)

def largest_app():
    sorted_by_size = df.sort_values(by='square_feet', ascending=False).head(10)
    
    data = {
        "labels": sorted_by_size['region'].tolist(),
        "datasets": [{
            "label": "Square Feet",
            "data": sorted_by_size['square_feet'].tolist()
        }]
    }
    return json.dumps(data)
    # fig, ax = plt.subplots(figsize=(11, 9))
    # ax.bar(sorted_by_size['region'], sorted_by_size['square_feet'])
    # ax.set_title('Top 10 Largest Appartments')
    # ax.set_xlabel('Square Feet')
    # ax.set_ylabel('Region');
    # ax.set_xticklabels(sorted_by_size['region'], rotation=45);
    # fig.savefig('Graphs/largest_app.png')

if __name__ == '__main__':
    largest_app()
    client.close()