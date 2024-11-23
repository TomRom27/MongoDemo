using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MongoDemo;

public class Product
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }
	public string Name { get; set; }
	public string MeasureUnit { get; set; }
	public int? MinimalOrder { get; set; }
	public ProductGroupReference Group { get; set; }

	// public bool? IsCertified { get; set; }

	[BsonIgnore]
	public string FullName => $"{Name} ({Group.Name})";

	public ProductReference GetReference()
	{
		return new ProductReference()
		{
			Id = this.Id,
			Name = this.Name,
			MeasureUnit = this.MeasureUnit
		};
	}
}


public class ProductReference
{
	public string Id { get; set; }
	public string Name { get; set; }
	public string MeasureUnit { get; set; }
}

public class ProductGroup
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }
	public string Name { get; set; }

	public ProductGroupReference GetReference()
	{
		return new ProductGroupReference()
		{
			Id = this.Id,
			Name = this.Name
		};
	}
}

public class ProductGroupReference
{
	public string Id { get; set; }
	public string Name { get; set; }
}

public enum OrderStatus
{
	Created,
	Approved,
	Completed,
	Cancelled
};

public class Order
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }
	public DateTime CreatedAt { get; set; }
	public OrderStatus Status { get; set; }
	public CustomerReference Customer { get; set; }
	public Address DeliveryAddress { get; set; }
	public string Notes { get; set; }
	public decimal Net { get; set; }
	public List<OrderLine> Lines { get; init; }

	public Order()
	{
		CreatedAt = DateTime.Now;
		Lines = new List<OrderLine>();
		Net = 0;
	}
	public void AddLine(OrderLine line)
	{
		line.Index = Lines.Count + 1;
		Lines.Add(line);
		Net += line.Value;
	}
}

public class OrderLine
{
	public int Index { get; set; }
	public ProductReference Product { get; set; }
	public decimal Quantity { get; set; }
	public decimal Price { get; set; }
	[BsonElement]
	public decimal Value => Math.Round(Quantity * Price, 2);
	//public void Value2() => Value = Math.Round(Quantity * Price, 2);
}

public class Customer
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }
	public string Name { get; set; }
	public Address Address { get; set; }
	public string Remarks { get; set; }
	public decimal OrderedSum { get; set; }

	public CustomerReference GetReference()
	{
		return new CustomerReference()
		{
			Id = this.Id,
			Name = this.Name,
			Address = this.Address
		};
	}
}

public class Address
{
	public string Street { get; set; }
	public string City { get; set; }
	public string ZipCode { get; set; }
}


public class CustomerReference
{
	public string Id { get; set; }
	public string Name { get; set; }
	public Address Address { get; set; }
}


