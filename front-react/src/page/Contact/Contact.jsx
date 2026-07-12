import React from 'react'
import './Contact.css'
import {APIProvider, Map} from '@vis.gl/react-google-maps';

function contact() {
  return (
    <div id='contactbody'>
      <div id='contactcontainer'>
        <div id='contactinfo'>
          <h1><b>İletişim:</b></h1>
            <p>Buğra Daryal</p>
            <p><a href="mailto:bugradaryal0@gmail.com">bugradaryal0@gmail.com</a></p>
            <p><a href="tel:+905333333333">+90 533 333 33 33</a></p>
            <p>Türkiye, Tekirdağ, Xxxxxx Mahallesi, Xxxxxxx Sokak, No: XX</p>
        </div>
        <div>
            <APIProvider apiKey={"AIzaSyDSazJqegeUTofZsKX6IYyxGLZ0LpkSyGs"}>
            <Map
              style={{width: '50vw', height: '50vh', border: '0.3em solid white', borderRadius: '0.5em'}}
              defaultCenter={{lat: 22.54992, lng: 0}}
              defaultZoom={3}
              gestureHandling='greedy'
              disableDefaultUI
            />
          </APIProvider>
        </div>
      </div>

    </div>
  )
}

export default contact