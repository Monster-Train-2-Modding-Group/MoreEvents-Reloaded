import csv
import json

def convert_csv_to_json(csv_file_path: str, json_file_path: str):
    # Mapping of target JSON keys to CSV header substrings/names
    language_keys = {
        "english": "English [en-US]",
        "french": "French [fr-FR]",
        "german": "German [de-DE]",
        "russian": "Russian",
        "portuguese": "Portuguese (Brazil)",
        "chinese": "Chinese [zh-CN]"
    }

    records = []

    with open(csv_file_path, mode='r', encoding='utf-8-sig') as csv_file:
        # csv.DictReader maps headers to row values and handles quoted text
        reader = csv.DictReader(csv_file)
        
        # Clean header keys by stripping whitespace
        reader.fieldnames = [field.strip() if field else field for field in reader.fieldnames]

        for row in reader:
            record = {}
            for json_key, header_name in language_keys.items():
                # Extract value and strip extraneous whitespace
                value = row.get(header_name, "")
                record[json_key] = value.strip() if value else ""
            
            records.append(record)

    with open(json_file_path, mode='w', encoding='utf-8') as json_file:
        json.dump(records, json_file, ensure_ascii=False, indent=2)

if __name__ == "__main__":
    # Update these paths to match your filenames
    input_csv = "translations.csv"
    output_json = "translations.json"
    
    convert_csv_to_json(input_csv, output_json)