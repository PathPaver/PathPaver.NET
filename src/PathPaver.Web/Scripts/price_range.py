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

def price_range():
    range = np.arange(0, 10000, 250)
    df_price_range = df.copy()
    df_price_range['price_range'] = pd.cut(df['price'], range)
    df_price_range = df_price_range.groupby('price_range').size().reset_index(name='count')

    data = {
        "labels": df_price_range['price_range'].astype(str).tolist(),
        "datasets": [{
            "label": "Number of Apartments",
            "data": df_price_range['count'].tolist(),
        }]
    }
    return json.dumps(data)

    # fig, ax = plt.subplots(figsize=(11, 9))
    # ax.bar(df_price_range['price_range'].astype(str), df_price_range['count'])
    # ax.set_title('Number of Apartments by Price Range')
    # ax.set_xlabel('Price Range')
    # ax.set_ylabel('Number of Apartments')
    # ax.set_xticks(df_price_range['price_range'].astype(str))
    # ax.set_xticklabels(df_price_range['price_range'].astype(str), rotation=45);
    # fig.savefig('Graphs/price_range.png')

if __name__ == '__main__':
    price_range()
    client.close()