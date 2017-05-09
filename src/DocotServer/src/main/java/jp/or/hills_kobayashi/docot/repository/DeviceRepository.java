package jp.or.hills_kobayashi.docot.repository;

import jp.or.hills_kobayashi.docot.model.Device;
import org.springframework.data.jpa.repository.JpaRepository;

import java.sql.Timestamp;
import java.util.List;

public interface DeviceRepository extends JpaRepository<Device, String> {

    public List<Device> findByPositionUpdatedGreaterThanEqualOrderByPositionUpdatedDesc(Timestamp positionUpdate);

}
