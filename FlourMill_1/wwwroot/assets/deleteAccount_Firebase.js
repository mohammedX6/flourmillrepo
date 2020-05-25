async function deleteBakeryFirebase(email) {
    console.log("Called");
  
    var firebaseId;
    firebase.firestore().collection("Users").where("email", "==",email)
    .get()
    .then(function(querySnapshot) {
        querySnapshot.forEach(function(doc) {
       
            console.log(doc.id, " => ", doc.data());
            firebaseId=doc.id;
            console.log("id after "  + firebaseId);

              firebase.firestore().collection("Bakerys_Location").doc(firebaseId).delete().then(function () {
                console.log("Account successfully deleted! Bakery Location");
            }).catch(function (error) {
                console.error("Error removing document: ", error);
            });
            firebase.firestore().collection("Users").doc(firebaseId).delete().then(function () {
                console.log("Account successfully deleted! Bakery User");
            }).catch(function (error) {
                console.error("Error removing document: ", error);
            });

        });
    })
    .catch(function(error) {
        console.log("Error getting documents: ", error);
    });
}

 function deleteAdminFirebase(id) {
    console.log("Called 2");
              firebase.firestore().collection("flourmill_location").doc(id+"").delete().then(function () {
                console.log("Account successfully deleted! Admin Location");
            }).catch(function (error) {
                console.error("Error removing document: ", error);
            });
}



async function deleteTruckDriver(email) {
   
              firebase.firestore().collection("user_locations").doc(email).delete().then(function () {
                console.log("Account successfully deleted! TruckDriver Location");
            }).catch(function (error) {
                console.error("Error removing document: ", error);
            });
            firebase.firestore().collection("Users").doc(email).delete().then(function () {
                console.log("Account successfully deleted! TruckDriver User");
            }).catch(function (error) {
                console.error("Error removing document: ", error);
            });


}