from pymongo import MongoClient
import pandas as pd
import matplotlib.pyplot as plt
import numpy as np
import sys
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


# ----------------------------------

pet_choice = widgets.Dropdown(
    options=['All', 'Cats', 'Dogs'],
    value='All',
    description='Pets allowed:',
)

is_furnished = widgets.Checkbox(
    value=False,
    description='Furnished'
)

beds_number = widgets.IntText(
    value=0,
    description='Beds (0 for any): ',
)

def cheapest_price(pet_type, furnished, beds):
    if pet_type == 'Cats':
        filtered_df = df[df['pets_allowed'].apply(lambda pets: 'Cats' in pets)]
    elif pet_type == 'Dogs':
        filtered_df = df[df['pets_allowed'].apply(lambda pets: 'Dogs' in pets)]
    else:
        filtered_df = df

    if furnished:
        filtered_df = filtered_df[filtered_df['furnished'] == True]

    if beds_number.value > 0:
        filtered_df = filtered_df[filtered_df['beds'] == beds]

    filtered_df = filtered_df[filtered_df['fee'] == False]
    df_grouped_by_region = filtered_df.groupby('region')
    df_average_price_per_region = df_grouped_by_region['price'].mean().sort_values(ascending=True).head(10)

    data = {
        "labels": df_average_price_per_region.index.tolist(),
        "datasets": [{
            "label": "Average Price",
            "data": df_average_price_per_region.tolist()
        }]
    }

    return json.dumps(data)

    # fig, ax = plt.subplots()
    # ax.bar(df_average_price_per_region.index, df_average_price_per_region)
    # ax.set_title('Top 10 Regions with Cheapest Average Price (no fees)')
    # ax.set_xlabel('Average Price Per Month')
    # ax.set_ylabel('Region')
    # plt.xticks(rotation=45)
    # fig.savefig('Graphs/cheapest_price.png')

if __name__ == '__main__':
    pet_choice = sys.argv[1]
    is_furnished = sys.argv[2]
    beds_number = sys.argv[3]
    cheapest_price(pet_choice, is_furnished, beds_number)
    client.close()