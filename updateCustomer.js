var conn = new Mongo();
var db = conn.getDB("TRDemo");

const now = new Date();

try {
	const google = db.customers.findOne({ name: { $regex: "Alphabet", $options: "i" } });
	if (!google) {
		throw new Error("customer not found");
	}

	google.remarks = "Updated via script at " +
		String(now.getHours()).padStart(2, '0') + ":" + String(now.getMinutes()).padStart(2, '0') + ":" + String(now.getSeconds()).padStart(2, '0');
	const result = db.customers.replaceOne({ name: google.name }, google);

	if (result.modifiedCount === 1) {
		print("Updated successfully!");
	} else {
		print("Update failed.");
	}
} catch (err) {
	print("Error:", err);
}
