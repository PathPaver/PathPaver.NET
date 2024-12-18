from pymongo import MongoClient
import pandas as pd
import os
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

def best_value():
    temp = df[(df['price'] > 0) & (df['square_feet'] > 0)]
    df_price_per_space = temp[['region', 'price', 'square_feet']].copy()
    df_price_per_space['price_per_sqft'] = df_price_per_space['price'] / df_price_per_space['square_feet']
    best_for_value = df_price_per_space.nsmallest(10, 'price_per_sqft')

    data = {
        "labels": best_for_value['region'].tolist(),
        "datasets": [{
            "label": "Price per square foot",
            "data": best_for_value['price_per_sqft'].tolist()
        }]
    }

    return json.dumps(data)
    # fig, ax = plt.subplots(figsize=(11, 9))
    # ax.bar(best_for_value['region'], best_for_value['price_per_sqft'])
    # ax.set_title('Top 10 Best Value per Square Foot by Region')
    # ax.set_ylabel('Price per Square Foot')
    # ax.set_xlabel('Region')
    # ax.set_xticks(best_for_value['region'])
    # ax.set_xticklabels(best_for_value['region'], rotation=45)
    # fig.savefig("Graphs/best_value.png")
    # print("Graph saved as best_value.png")

if __name__ == '__main__':
    best_value()
    client.close()