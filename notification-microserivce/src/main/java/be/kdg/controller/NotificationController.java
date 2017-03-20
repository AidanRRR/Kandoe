package be.kdg.controller;

import be.kdg.service.NotificationService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.messaging.simp.SimpMessagingTemplate;
import org.springframework.web.bind.annotation.*;

/**
 * Created by mathi on 9/02/2017.
 */
@RestController
@RequestMapping("/notification")
public class NotificationController {

    @Autowired
    NotificationService notificationService;


    @RequestMapping(value = "/send-notification", method = RequestMethod.POST)
    public void sendNotification(@RequestParam String message, @RequestParam String userName) throws InterruptedException {
        notificationService.sendNotification(message, userName);
    }

}
