function deleteBakeryFirebase(id) {


    db.collection("Bakerys_Location").doc("DC").delete().then(function () {
        console.log("Account successfully deleted! Location Bakery");
    }).catch(function (error) {
        console.error("Error removing document: ", error);
    });
    db.collection("Users").doc("DC").delete().then(function () {
        console.log("Account successfully deleted! Bakery ");
    }).catch(function (error) {
        console.error("Error removing document: ", error);
    });

}

function deleteAdminFirebase(id) {
    db.collection("flourmill_location").doc(id).delete().then(function () {
        console.log("Account successfully deleted! Admin");
    }).catch(function (error) {
        console.error("Error removing document:", error);
    });

}
function deleteTruckDriver(id) {
    db.collection("user_locations").doc("DC").delete().then(function () {
        console.log("Account successfully deleted! location TruckDriver");
    }).catch(function (error) {
        console.error("Error removing document: ", error);
    });

    db.collection("Users").doc("DC").delete().then(function () {
        console.log("Account successfully deleted! Truck Driver");
    }).catch(function (error) {
        console.error("Error removing document: ", error);
    });

}