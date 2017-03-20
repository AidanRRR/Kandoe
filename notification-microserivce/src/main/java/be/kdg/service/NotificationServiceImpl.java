package be.kdg.service;

import be.kdg.model.Message;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.messaging.simp.SimpMessagingTemplate;
import org.springframework.stereotype.Service;

import java.io.IOException;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.ProtocolException;
import java.net.URL;

/**
 * Created by mathi on 9/02/2017.
 */
@Service
public class NotificationServiceImpl implements NotificationService {

    private static String SERVER_KEY = "AAAAfxAwWGE:APA91bHZMEd06DpDi4u8RmmXW1kJk2VD5AHZRNVP8l6-CmOqR8ULWVinxF6SRgPPwos2RH9daGRMNmqiqJ-V1dHvBSZSC10A0sgXbCIR4lOjwXNptYt-9wOD0Na-FZbnYBUa0Dsr1G2H";

    @Autowired
    private SimpMessagingTemplate template;

    @Autowired
    private DeviceService deviceService;

    @Override
    public void sendNotification(String message, String userName){

        /**
         * SEND BROWSER
         */
        deviceService.findByUserId(userName);
        this.template.convertAndSendToUser(userName,"/new-notification", new Message(message));

        /**
         * SEND PUSH
         */
        String pushMessage = "{\"data\":{\"title\":\"" +
                "Kandoe notification" +
                "\",\"message\":\"" +
                message +
                "\"},\"to\":\"" +
                deviceService.findByUserId(userName).getDeviceId() +
                "\"}";
        // Create connection to send FCM Message request.
        try {
            URL url = new URL("https://fcm.googleapis.com/fcm/send");
            HttpURLConnection conn = (HttpURLConnection) url.openConnection();
            conn.setRequestProperty("Authorization", "key=" + SERVER_KEY);
            conn.setRequestProperty("Content-Type", "application/json");
            conn.setRequestMethod("POST");
            conn.setDoOutput(true);

            // Send FCM message content.
            OutputStream outputStream = conn.getOutputStream();
            outputStream.write(pushMessage.getBytes());

            System.out.println(conn.getResponseCode());
            System.out.println(conn.getResponseMessage());
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
