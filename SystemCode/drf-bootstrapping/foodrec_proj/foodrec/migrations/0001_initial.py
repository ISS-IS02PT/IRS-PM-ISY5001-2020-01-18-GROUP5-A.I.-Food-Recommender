# Generated by Django 3.0.3 on 2020-03-01 13:45

from django.db import migrations, models


class Migration(migrations.Migration):

    initial = True

    dependencies = [
    ]

    operations = [
        migrations.CreateModel(
            name='Food',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('name', models.CharField(max_length=100)),
                ('group', models.CharField(choices=[('BEVERAGES', 'BEVERAGES'), ('CEREAL AND CEREAL PRODUCTS', 'CEREAL AND CEREAL PRODUCTS'), ('EGG AND EGG PRODUCTS', 'EGG AND EGG PRODUCTS'), ('FAST FOODS', 'FAST FOODS'), ('FISH AND FISH PRODUCTS', 'FISH AND FISH PRODUCTS'), ('FRUIT AND FRUIT PRODUCTS', 'FRUIT AND FRUIT PRODUCTS'), ('HEALTHIER CHOICE SYMBOL (HCS) PRODUCTS', 'HEALTHIER CHOICE SYMBOL (HCS) PRODUCTS'), ('MEAT AND MEAT PRODUCTS', 'MEAT AND MEAT PRODUCTS'), ('MILK AND MILK PRODUCTS', 'MILK AND MILK PRODUCTS'), ('MISCELLANEOUS', 'MISCELLANEOUS'), ('MIXED ETHNIC DISHES, ANALYZED IN SINGAPORE', 'MIXED ETHNIC DISHES, ANALYZED IN SINGAPORE'), ('NUTS AND SEEDS, PULSES AND PRODUCTS', 'NUTS AND SEEDS, PULSES AND PRODUCTS'), ('OILS AND FATS', 'OILS AND FATS'), ('OTHER MIXED ETHNIC DISHES', 'OTHER MIXED ETHNIC DISHES'), ('SUGARS, SWEETS AND CONFECTIONERY', 'SUGARS, SWEETS AND CONFECTIONERY'), ('VEGETABLE AND VEGETABLE PRODUCTS', 'VEGETABLE AND VEGETABLE PRODUCTS')], default='MISCELLANEOUS', max_length=100)),
                ('nutrient_protein_amount', models.FloatField()),
                ('nutrient_protein_unit', models.CharField(max_length=10)),
                ('nutrient_fat_amount', models.FloatField()),
                ('nutrient_fat_unit', models.CharField(max_length=10)),
            ],
            options={
                'ordering': ['name'],
            },
        ),
    ]
