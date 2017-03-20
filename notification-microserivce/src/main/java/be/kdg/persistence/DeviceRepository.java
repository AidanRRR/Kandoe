package be.kdg.persistence;

import be.kdg.model.Device;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

/**
 * Created by mathi on 9/02/2017.
 */
public interface DeviceRepository extends JpaRepository<Device,Integer>{

    Device findByUserId(String userId);



}
