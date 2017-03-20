package be.kdg.model;

import javax.persistence.*;

/**
 * Created by mathi on 9/02/2017.
 */
@Entity
@Table(name = "device")
public class Device {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id")
    private long id;

    @Column(name = "userId")
    private String userId;

    @Column(name = "deviceId")
    private String deviceId;

    protected Device() {
    }

    public Device(String userId, String deviceId) {
        this.userId = userId;
        this.deviceId = deviceId;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public String getDeviceId() {
        return deviceId;
    }

    public void setDeviceId(String deviceId) {
        this.deviceId = deviceId;
    }
}
