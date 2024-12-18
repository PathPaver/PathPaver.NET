from pymongo import MongoClient
import pandas as pd
import matplotlib.pyplot as plt
import numpy as np
import json
import ipywidgets as widgets
from dotenv import dotenv_values

config = dotenv_values(".env")

client = MongoClient(config["DB_URI"])

try:
    client.admin.command('ping')
    print("Connection réussie!")
    db = client.PATH_PAVER_DEV
    collection = db.rents
    df = pd.DataFrame(list(collection.find()));
except Exception as e:
    print(e)

def app_count():
    df_region_count = df['region'].value_counts().head(10)

    data = {
        "labels": df_region_count.index.tolist(),
        "datasets": [{
            "label": "Number of Apartments",
            "data": df_region_count.tolist()
        }]
    }

    return json.dumps(data)
    # fig, ax = plt.subplots(figsize=(11, 9))
    # ax.bar(df_region_count.index, df_region_count)
    # ax.set_title('Top 10 Regions with the Most Apartments')
    # ax.set_xticks(df_region_count.index)
    # ax.set_xticklabels(df_region_count.index, rotation=45)
    # ax.set_xlabel('Region')
    # ax.set_ylabel('Number of Apartments');
    # fig.savefig('Graphs/app_count.png')
    # print("Graph saved as app_count.png")

if __name__ == '__main__':
    app_count()
    client.close()