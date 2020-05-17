var directionsService;
var directionsRenderer;
var intervalx;
function initMap(FirstOrigin,FirstDestination,id) {

  
  clearInterval(intervalx);
     directionsService = new google.maps.DirectionsService();
     directionsRenderer = new google.maps.DirectionsRenderer();
    var map = new google.maps.Map(document.getElementById('map'), {
      zoom: 7,
      disableAutoPan: true,
    

      center: {lat: 32.140734, lng: 36.154633}
    });

    var UpdatedGeoPointLAT;
    var UpdatedGeoPointLONG;
    var UpdatedGeoPoint;
    
    directionsRenderer.setMap(map);
    intervalx=  setInterval(function(){

      output =firebase.firestore().collection("user_locations").doc(id);
      output.get().then(function(doc) {
        if (doc.exists) {
            console.log("Document data:", doc.data());
            UpdatedGeoPointLAT=doc.data().geo_point.latitude;
            UpdatedGeoPointLONG=doc.data().geo_point.longitude;
            UpdatedGeoPoint=UpdatedGeoPointLAT+","+UpdatedGeoPointLONG;
            console.log("geopoint "+UpdatedGeoPointLAT);
            console.log("geopoint "+UpdatedGeoPointLONG);
           calculateAndDisplayRoute(directionsService, directionsRenderer,UpdatedGeoPoint,FirstDestination);
        } else {
       
            console.log("not found");
        }
    }).catch(function(error) {
        console.log("Error ", error);
    });
  },1000) 
  }



  function calculateAndDisplayRoute(directionsService, directionsRenderer,FirstOrigin,FirstDestination) {

    console.log("called");
    directionsService.route(
        {
          origin: {query: FirstOrigin},
          destination: {query: FirstDestination},
          travelMode: 'DRIVING'
          
        },
        function(response, status) {
          if (status === 'OK') {
            directionsRenderer.setDirections(response);
          } else {
            console.log("error");
          }
        });

       
       
  }